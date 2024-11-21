import { PropsWithChildren, useState } from "react";
import styles from "./PageTemplate.module.scss";
import { ScoreTile } from "@/components/ScoreTile";
import { Timer } from "@/components/Timer";
import { Latency } from "@/components/Latency";
import { FlyingSquare } from "@/components/FlyingSquare";

type Props = {
  initialTimerSeconds?: number;
} & PropsWithChildren;

const PageTemplate = ({ children, initialTimerSeconds }: Props) => {
  const [showTimer, setShowTimer] = useState((initialTimerSeconds ?? 0) > 0);

  return (
    <main className={styles.page}>
      <div className={styles.topRight}>
        <Latency />
        <ScoreTile />
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
      <FlyingSquare count={5} />
    </main>
  );
};

export default PageTemplate;
