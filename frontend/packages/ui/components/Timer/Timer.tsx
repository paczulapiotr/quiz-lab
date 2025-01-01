import React, { useState, useEffect } from "react";
import styles from "./Timer.module.scss";
import classNames from "classnames";

type Props = {
  startSeconds: number; // Total countdown time in seconds
  onTimeUp?: () => void; // Callback function when time is up
};

const Timer: React.FC<Props> = ({ startSeconds, onTimeUp }) => {
  const [timeLeft, setTimeLeft] = useState(startSeconds);
  const [percentage, setPercentage] = useState(0);

  useEffect(() => {
    let interval: NodeJS.Timeout;
    if (timeLeft > 0) {
      interval = setInterval(() => {
        setTimeLeft((prevTime) => prevTime - 1);
      }, 1_000);
    } else {
      setTimeout(() => {
        onTimeUp?.();
      }, 100);
    }

    return () => { interval != null && clearInterval(interval); }
  }, [timeLeft, onTimeUp]);

  useEffect(() => {
    // Calculate the percentage of time passed
    const timeElapsed = startSeconds - (timeLeft - 1);
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
