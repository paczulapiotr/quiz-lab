import React, { memo, useEffect, useRef } from "react";
import { flySquare } from "./helpers";
import styles from "./FlyingSquare.module.scss";

interface FlyingSquareProps {
  count?: number;
}

const FlyingSquare: React.FC<FlyingSquareProps> = ({ count = 1 }) => {
  const squareRefs = useRef<HTMLDivElement[]>([]);

  useEffect(() => {
    squareRefs.current.forEach((square) => {
      if (square) {
        flySquare(square);
      }
    });
  }, [count]);

  return (
    <div>
      {Array.from({ length: count }).map((_, index) => (
        <div
          key={index}
          ref={(el) => {
            if (el) squareRefs.current[index] = el;
          }}
          className={styles.square}
        ></div>
      ))}
    </div>
  );
};

export default memo(FlyingSquare);
