import { Tile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import { IncorrectLetters} from "@repo/ui/components/minigames/LettersAndPhrases/IncorrectLetters";
import { memo } from "react";

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
    <>
      {playerAnswering ? (
        <Tile blue text={`Odpowiada: ${playerAnswering}`} />
      ) : null}
      <div style={{ flex: 1, display: "flex", alignItems: "center" }}>
        <Phrase phrase={phrase} usedLetters={usedLetters} />
      </div>
      <IncorrectLetters letters={incorrectLetters} />
      {startSeconds ? (
        <Timer startSeconds={startSeconds} onTimeUp={onTimeUp} key={timerKey} />
      ) : null}
    </>
  );
};

export default memo(Round);
