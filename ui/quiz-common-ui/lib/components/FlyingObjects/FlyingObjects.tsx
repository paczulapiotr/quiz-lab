import React, { memo, PropsWithChildren, useEffect, useRef } from "react";
import { flySquare } from "./helpers";
import styles from "./FlyingObjects.module.scss";
import classNames from "classnames";

type FlyingObjectsProps = {
  count?: number;
  speed?: number;
  rotationSpeed?: number;
  className?: string;
} & Partial<PropsWithChildren>;

const FlyingObjects: React.FC<FlyingObjectsProps> = ({
  count = 1,
  rotationSpeed = 1,
  speed = 1,
  className,
  children,
}) => {
  const squareRefs = useRef<HTMLDivElement[]>([]);

  useEffect(() => {
    squareRefs.current.forEach((square) => {
      if (square) {
        flySquare(square, speed);
      }
    });
  }, [count, speed]);

  return (
    <div>
      {Array.from({ length: count }).map((_, index) => (
        <div
          key={index}
          ref={(el) => {
            if (el) squareRefs.current[index] = el;
          }}
          className={classNames(className, styles.item)}
          style={{ animationDuration: `${10 / rotationSpeed}s` }}
        >
          {children}
        </div>
      ))}
    </div>
  );
};

export default memo(FlyingObjects);
