import classNames from "classnames";
import styles from "./Phrase.module.scss";
import { memo } from "react";

type Props = {
  usedLetters?: string[];
  phrase: string[];
  className?: string;
  solved?: boolean;
};

const Phrase = ({ usedLetters, phrase, className, solved }: Props) => {
  return (
    <div className={classNames(styles.phrase, className)}>
      {phrase.map((word, i) => (
        <div className={styles.word} key={`${i}_${word}`}>
          {word.split("").map((x, j) => {
            const guessed = solved || usedLetters?.includes(x.toLowerCase());
            return (
              <div
                key={`${i}_${j}_${x}`}
                className={classNames(styles.letter, {
                  [styles.guessed]: guessed,
                })}
              >
                {x.toUpperCase()}
              </div>
            );
          })}
        </div>
      ))}
    </div>
  );
};

export default memo(Phrase);
