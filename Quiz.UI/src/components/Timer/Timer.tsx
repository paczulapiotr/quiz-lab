import React, { useState, useEffect } from "react";
import styles from "./Timer.module.scss";
import classNames from "classnames";

type Props = {
  startSeconds: number; // Total countdown time in seconds
};

const Timer: React.FC<Props> = ({ startSeconds }) => {
  const [timeLeft, setTimeLeft] = useState(startSeconds);
  const [percentage, setPercentage] = useState(0);

  useEffect(() => {
    if (timeLeft > 0) {
      const interval = setInterval(() => {
        setTimeLeft((prevTime) => prevTime - 1);
      }, 1000);
      return () => clearInterval(interval); // Clear the interval on unmount
    }
  }, [timeLeft]);

  useEffect(() => {
    // Calculate the percentage of time passed
    const timeElapsed = startSeconds - timeLeft;
    setPercentage((timeElapsed / startSeconds) * 100);
  }, [timeLeft, startSeconds]);

  // Determine color based on the percentage
  const getProgressColor = () => {
    if (percentage < 33) return "green";
    if (percentage < 66) return "yellow";
    return "red";
  };

  return (
    <div className={styles.timer}>
      <div className={styles["progress-bar"]}>
        <div
          style={{ width: `${percentage}%` }}
          className={classNames(
            styles["progress-fill"],
            styles[getProgressColor()],
          )}
        />
      </div>
      <div className={styles.display}>
        {new Date(timeLeft * 1000).toISOString().substr(14, 5)}
      </div>
    </div>
  );
};
export default Timer;
