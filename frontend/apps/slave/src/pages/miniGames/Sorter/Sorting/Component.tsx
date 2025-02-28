import React, { useState, useRef } from "react";
import ItemSorter, { Item } from "@repo/ui/components/ItemSorter";
import classNames from "classnames";
import styles from "./Component.module.scss";

export type Props = {
  leftAnswer: string;
  rightAnswer: string;
  onLeftAnswer: (item: Item) => void;
  onRightAnswer: (item: Item) => void;
  items: Item[];
};

const Component = ({ items, leftAnswer, rightAnswer, onLeftAnswer, onRightAnswer }: Props) => {
  const [highlightedLeft, setHighlightedLeft] = useState<
    "green" | "red" | null
  >(null);
  const timeoutLeftRef = useRef<NodeJS.Timeout | null>(null);

  const [highlightedRight, setHighlightedRight] = useState<
    "green" | "red" | null
  >(null);
  const timeoutRightRef = useRef<NodeJS.Timeout | null>(null);

  // Simplified helper: sets the highlight color based on the item's flag.
  const triggerHighlight = (
    side: "LEFT" | "RIGHT",
    setter: React.Dispatch<React.SetStateAction<"green" | "red" | null>>,
    timeoutRef: React.MutableRefObject<NodeJS.Timeout | null>,
    item: Item,
  ) => {
    const color =
      side === "LEFT"
        ? item.left
          ? "green"
          : "red"
        : item.right
          ? "green"
          : "red";
    setter(color);
    if (timeoutRef.current) clearTimeout(timeoutRef.current);
    timeoutRef.current = setTimeout(() => {
      setter(null);
    }, 400);
    console.log(`${side} ${item.name} (${color})`);
  };

  const handleAssignLeft = (item: Item) => {
    triggerHighlight("LEFT", setHighlightedLeft, timeoutLeftRef, item);
    onLeftAnswer(item);
  };

  const handleAssignRight = (item: Item) => {
    triggerHighlight("RIGHT", setHighlightedRight, timeoutRightRef, item);
    onRightAnswer(item);
  };

  return (
    <div className={styles.page}>
      <ItemSorter
        children={(item, drag) => (
          <div
            className={styles.item}
            style={{
              transform: drag ? "scale(1.1)" : undefined,
            }}
          >
            <span className={styles.text}>{item.name}</span>
          </div>
        )}
        left={
          <div className={styles.leftContainer}>
            <h1
              className={classNames(styles.title, {
                [styles.green]: highlightedLeft === "green",
                [styles.red]: highlightedLeft === "red",
              })}
            >
              {leftAnswer}
            </h1>
          </div>
        }
        right={
          <div className={styles.rightContainer}>
            <h1
              className={classNames(styles.title, {
                [styles.green]: highlightedRight === "green",
                [styles.red]: highlightedRight === "red",
              })}
            >
              {rightAnswer}
            </h1>
          </div>
        }
        items={items}
        onAllSorted={() => console.log("ALL SORTED")}
        onAssignLeft={handleAssignLeft}
        onAssignRight={handleAssignRight}
      />
    </div>
  );
};

export default Component;
