import { Keyboard } from "@/components/Keyboard";
import { ScoreTile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import { IncorrectLetters } from "@repo/ui/components/minigames/LettersAndPhrases/IncorrectLetters";
import styles from './Component.module.scss'
import { memo } from "react";
import Times from "@repo/ui/config/times";

type Props = {
  onSelect: (letter: string) => void;
  phrase: string[];
  usedLetters: string[];
  incorrectLetters: string[];
  score: number;
  yourTurn?: boolean;
  timerKey?: React.Key;
};

const Component = ({
  onSelect,
  score,
  phrase,
  usedLetters,
  yourTurn,
  incorrectLetters,
  timerKey,
}: Props) => {

  return (
    <>
      <ScoreTile score={score} />
      <div className={styles.phrase}>
        <Phrase phrase={phrase} usedLetters={usedLetters} />
        <IncorrectLetters
          letters={incorrectLetters}
          className={styles.incorrect}
        />
      </div>
      {yourTurn ? (
        <div className={styles.keyboard}>
          <Keyboard
            onKeyPress={onSelect}
            disableBackspace
            disableShift
            disableSpaceBar
            defaultUpperCase
            disabledLetters={usedLetters}
          />
          <Timer startSeconds={Times.Letters.AnswerSeconds} key={timerKey} />
        </div>
      ) : null}
    </>
  );
};

export default memo(Component);
