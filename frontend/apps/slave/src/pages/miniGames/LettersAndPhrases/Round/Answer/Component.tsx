import { Keyboard } from "@/components/Keyboard";
import { ScoreTile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import { IncorrectLetters } from "@repo/ui/components/minigames/LettersAndPhrases/IncorrectLetters";
import styles from './Component.module.scss'
import { memo } from "react";

type Props = {
  onSelect: (letter: string) => void;
  phrase: string[];
  usedLetters: string[];
  incorrectLetters: string[];
  score: number;
  yourTurn?: boolean;
};

const Component = ({
  onSelect,
  score,
  phrase,
  usedLetters,
  yourTurn,
  incorrectLetters,
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
        <>
          <Keyboard
            onKeyPress={onSelect}
            disableBackspace
            disableShift
            disableSpaceBar
            defaultUpperCase
            disabledLetters={usedLetters}
          />
          <Timer startSeconds={30} />
        </>
      ) : null}
    </>
  );
};

export default memo(Component);
