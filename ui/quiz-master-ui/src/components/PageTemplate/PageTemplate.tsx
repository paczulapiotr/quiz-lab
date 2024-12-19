import { PropsWithChildren, useState } from "react";
import styles from "./PageTemplate.module.scss";
import { ScoreTile } from "@/components/ScoreTile";
import { Latency, Timer } from "quiz-common-ui/components";

type Props = {
  initialTimerSeconds?: number;
  score?: number;
} & PropsWithChildren;

const PageTemplate = ({ children, initialTimerSeconds, score }: Props) => {
  const [showTimer, setShowTimer] = useState((initialTimerSeconds ?? 0) > 0);

  return (
    <main className={styles.page}>
      <div className={styles.topRight}>
        <Latency />
        {score !== undefined && <ScoreTile score={score} />}
      </div>
      <div className={styles.container}>
        {initialTimerSeconds && showTimer ? (
          <div className={styles.timer}>
            <Timer
              startSeconds={initialTimerSeconds}
              onTimeUp={() => setShowTimer(false)}
            />
          </div>
        ) : (
          <div className={styles.content}>{children}</div>
        )}
      </div>
      {/* <FlyingSquare count={5} /> */}
    </main>
  );
};

export default PageTemplate;
