import classNames from "classnames";
import styles from "./IncorrectLetters.module.scss";
import { memo } from "react";

type Props = {
  className?: string;
  letters: (string | null)[];
};

const IncorrectLetters = ({ letters, className }: Props) => {
  return (
    <div className={classNames(className, styles.letters)}>
      {letters
        .filter((x) => x != null)
        .map((letter) => (
          <div className={styles.letter} key={letter}>
            {letter!.toUpperCase()}
          </div>
        ))}
    </div>
  );
};

export default memo(IncorrectLetters);
