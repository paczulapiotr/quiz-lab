import classNames from "classnames";
import styles from "./Phrase.module.scss";

type Props = {
  usedLetters?: string[];
  phrase: string[];
  className?: string;
  solved?: boolean;
};

const Phrase = ({ usedLetters, phrase, className, solved }: Props) => {
  return (
    <div className={classNames(styles.phrase, className)}>
      {phrase.map((word) => (
        <div className={styles.word}>
          {word.split("").map((x) => {
            const guessed = solved || usedLetters?.includes(x.toLowerCase());
            return (
              <div
                className={classNames(styles.letter, {
                  [styles.guessed]: guessed,
                })}
              >
                {guessed ? x.toUpperCase() : ""}
              </div>
            );
          })}
        </div>
      ))}
    </div>
  );
};

export default Phrase;
