import React, { useRef, useState, useMemo } from "react";
import { motion, AnimatePresence, PanInfo } from "framer-motion";
import styles from "./index.module.scss";

export type Item = {
  id: string;
  name: string;
}

type Alignment = "left" | "right" | "center";

interface DragBetweenColumnsProps {
  left: React.ReactElement;
  right: React.ReactElement;
  items: Item[];
  onAssignLeft: (item: Item) => void;
  onAssignRight: (item: Item) => void;
  onAllSorted: () => void;
  children: (item: Item, isDragged: boolean) => React.ReactElement;
}

const ItemSorter: React.FC<DragBetweenColumnsProps> = ({
  left,
  right,
  items,
  onAssignLeft,
  onAssignRight,
  onAllSorted,
  children,
}) => {
  const leftRef = useRef<HTMLDivElement>(null);
  const rightRef = useRef<HTMLDivElement>(null);
  const containerRef = useRef<HTMLDivElement>(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [alignemnt, setAlignment] = useState<Alignment[]>([]);
  const lastIndex = useMemo(() => items.length - 1, [items]);
  const [isDragging, setIsDragging] = useState(false);

  const handleDragEnd = (
    _: MouseEvent | TouchEvent | PointerEvent,
    info: PanInfo,
  ) => {
    setIsDragging(false);
    const leftRects = leftRef.current?.getBoundingClientRect();
    const rightRects = rightRef.current?.getBoundingClientRect();
    const leftConstraint = leftRects ? leftRects.x + leftRects.width : 0;
    const rightConstraint = rightRects?.x ?? 0;

    const newAlign =
      info.point.x >= rightConstraint
        ? "right"
        : info.point.x <= leftConstraint
          ? "left"
          : "center";
    if (newAlign === "center") return;

    handleAssignment(items[currentIndex], newAlign);

    if (currentIndex >= lastIndex) {
      onAllSorted();
    }
  };

  const handleAssignment = (item: Item, newAlign: Alignment) => {
    setAlignment((prev) => [...prev, newAlign]);

    if (newAlign === "right") {
      onAssignRight(item);
    } else if (newAlign === "left") {
      onAssignLeft(item);
    }

    setTimeout(() => {
      setCurrentIndex((prev) => prev + 1);
    }, 250);
  };

  const calcXAlignment = (alignment: Alignment) => {
    const halfWidth =
      (containerRef.current?.getBoundingClientRect().width ?? 0) / 2;
    const leftWidth = (leftRef.current?.getBoundingClientRect().width ?? 0) / 2;
    const rightWidth =
      (rightRef.current?.getBoundingClientRect().width ?? 0) / 2;

    return alignment === "left"
      ? -(halfWidth - leftWidth)
      : alignment === "right"
        ? halfWidth - rightWidth
        : 0;
  };

  return (
    <div
      ref={containerRef}
      className={styles.mainContainer}
    >
      <div ref={leftRef} className={styles.leftContainer}>
        {left}
      </div>
      <AnimatePresence>
        {items.map((item, index) => {
          const itemAlignment = alignemnt[index] ?? "center";
          const xAlign = calcXAlignment(itemAlignment);
          return currentIndex === index ? (
            <motion.div
              key={index}
              drag={currentIndex === index}
              dragConstraints={containerRef}
              onDragStart={() => setIsDragging(true)}
              onDragEnd={handleDragEnd}
              className={styles.draggable}
              initial={{ opacity: 0 }}
              exit={{ opacity: 0, x: xAlign, y: 0 }}
              dragSnapToOrigin={itemAlignment === "center"}
              animate={{ x: xAlign, y: 0, opacity: 1 }}
              transition={{ type: "spring", stiffness: 300, damping: 30 }}
            >
              {children(item, isDragging)}
            </motion.div>
          ) : null;
        })}
      </AnimatePresence>

      <div ref={rightRef} className={styles.rightContainer}>
        {right}
      </div>
    </div>
  );
};

export default ItemSorter;
