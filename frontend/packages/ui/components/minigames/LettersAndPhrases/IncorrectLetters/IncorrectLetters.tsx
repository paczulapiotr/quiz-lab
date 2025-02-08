import classNames from "classnames";
import styles from "./IncorrectLetters.module.scss";
import { memo } from "react";

type Props = {
  className?: string;
  letters: string[];
};

const IncorrectLetters = ({ letters, className }: Props) => {
  return (
    <div className={classNames(className, styles.letters)}>
      {letters.map((letter) => (
        <div className={styles.letter} key={letter}>
          {letter.toUpperCase()}
        </div>
      ))}
    </div>
  );
};

export default memo(IncorrectLetters);
