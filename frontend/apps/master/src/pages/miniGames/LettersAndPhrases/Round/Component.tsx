import { Tile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import { IncorrectLetters } from "@repo/ui/components/minigames/LettersAndPhrases/IncorrectLetters";
import styles from "./Component.module.scss";

type Props = {
  phrase: string[];
  usedLetters: string[];
  startSeconds?: number;
  incorrectLetters: string[];
  onTimeUp?: () => void;
  playerAnswering?: string;
  timerKey?: React.Key;
};

const Round = ({
  phrase,
  usedLetters,
  onTimeUp,
  startSeconds,
  incorrectLetters,
  playerAnswering,
  timerKey,
}: Props) => {
  return (
    <div className={styles.page}>
      {playerAnswering ? (
        <Tile blue text={`Odpowiada: ${playerAnswering}`} />
      ) : null}
      <Phrase phrase={phrase} usedLetters={usedLetters} />
      <div>
        <IncorrectLetters letters={incorrectLetters} />
        {startSeconds ? (
          <Timer
            startSeconds={startSeconds}
            onTimeUp={onTimeUp}
            key={timerKey}
          />
        ) : null}
      </div>
    </div>
  );
};

export default Round;
