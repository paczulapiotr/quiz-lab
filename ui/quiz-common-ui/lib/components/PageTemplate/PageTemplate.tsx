import { PropsWithChildren, useState } from "react";
import styles from "./PageTemplate.module.scss";
import { FlyingSquare, Timer } from "quiz-common-ui/components";
import { motion as m } from "motion/react";
import classNames from "classnames";

type Props = {
  initialTimerSeconds?: number;
  squares?: boolean;
  className?: string;
} & PropsWithChildren;

const PageTemplate = ({
  children,
  initialTimerSeconds,
  squares,
  className,
}: Props) => {
  const [showTimer, setShowTimer] = useState((initialTimerSeconds ?? 0) > 0);

  return (
    <m.main
      className={classNames(styles.page, className)}
      initial={{ opacity: 0, x: "100%", backgroundColor: "rgba(70,120,255,1)" }}
      animate={{ opacity: 1, x: "0%", backgroundColor: "rgba(70,120,255,0)" }}
      transition={{ duration: 0.75, ease: "easeOut" }}
      exit={{
        opacity: 0,
        x: "-100%",
        transition: { duration: 0.75, ease: "easeOut" },
      }}
    >
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
      {squares ? <FlyingSquare count={5} /> : null}
    </m.main>
  );
};

export default PageTemplate;
