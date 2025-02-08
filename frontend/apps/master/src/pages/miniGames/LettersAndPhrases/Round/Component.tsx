import { Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import { IncorrectLetters} from "@repo/ui/components/minigames/LettersAndPhrases/IncorrectLetters";

type Props = {
  phrase: string[];
  usedLetters: string[];
  startSeconds?: number;
  incorrectLetters: string[];
  onTimeUp?: () => void;
};

const Round = ({
  phrase,
  usedLetters,
  onTimeUp,
  startSeconds,
  incorrectLetters,
}: Props) => {
  return (
    <>
      <div style={{ flex: 1, display: "flex", alignItems: "center" }}>
        <Phrase phrase={phrase} usedLetters={usedLetters} />
      </div>
      <IncorrectLetters letters={incorrectLetters}/>
      {startSeconds && onTimeUp ? (
        <Timer startSeconds={startSeconds} onTimeUp={onTimeUp} />
      ) : null}
    </>
  );
};

export default Round;
