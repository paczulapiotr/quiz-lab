import React, { useState, useEffect, useRef, useCallback } from "react";
import styles from "./Timer.module.scss";
import classNames from "classnames";

type Props = {
  startSeconds: number; // Total countdown time in seconds
  onTimeUp?: () => void; // Callback function when time is up
};

const Timer: React.FC<Props> = ({ startSeconds, onTimeUp }) => {
  const [timeLeft, setTimeLeft] = useState(startSeconds);
  const [percentage, setPercentage] = useState(0);
  const intervalRef = useRef<NodeJS.Timeout | null>(null);
  const onTimeUpCalled = useRef(false);
  const endTimeRef = useRef<number>(Date.now() + startSeconds * 1000);

  const updateTimer = useCallback(() => {
    const timeRemaining = Math.max(0, endTimeRef.current - Date.now());
    const secondsLeft = Math.ceil(timeRemaining / 1000);
    setTimeLeft(secondsLeft);

    if (timeRemaining <= 0 && !onTimeUpCalled.current) {
      onTimeUpCalled.current = true;
      onTimeUp?.();
      clearInterval(intervalRef.current!);
    }

    // Calculate the percentage of time passed
    const timeElapsed = startSeconds * 1000 - timeRemaining;
    setPercentage((timeElapsed / (startSeconds * 1000)) * 100);
  },[onTimeUp, startSeconds])

  useEffect(() => {
    updateTimer(); // Invoke immediately to set initial state

    intervalRef.current = setInterval(updateTimer, 100);

    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current);
      }
    };
  }, [startSeconds, onTimeUp, updateTimer]);

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
          style={{ width: `${percentage}%`}}
          className={classNames(
            styles["progress-fill"],
            styles[getProgressColor()]
          )}
        />
      </div>
      <div className={styles.display}>
        {`${Math.floor(timeLeft / 60)
          .toString()
          .padStart(2, "0")}:${(timeLeft % 60).toString().padStart(2, "0")}`}
      </div>
    </div>
  );
};

export default Timer;