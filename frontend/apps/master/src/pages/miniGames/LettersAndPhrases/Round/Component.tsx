import { Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";

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
      <div style={{ marginBottom: "auto" }}>
        <Phrase phrase={phrase} usedLetters={usedLetters} />
        <p
          style={{ color: "red" }}
        >{`Incorrect: ${incorrectLetters.join(", ")}`}</p>
      </div>
      {startSeconds && onTimeUp ? (
        <Timer startSeconds={startSeconds} onTimeUp={onTimeUp} />
      ) : null}
    </>
  );
};

export default Round;
