import React, { memo } from "react";
import { FlyingObjects } from "../FlyingObjects";
import styles from "./FlyingSquare.module.scss";
interface FlyingSquareProps {
  count?: number;
}

const FlyingSquare: React.FC<FlyingSquareProps> = ({ count = 1 }) => {
  return <FlyingObjects count={count} className={styles.square} />;
};

export default memo(FlyingSquare);
