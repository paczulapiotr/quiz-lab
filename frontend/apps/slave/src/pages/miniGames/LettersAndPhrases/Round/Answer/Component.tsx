import { Keyboard } from "@/components/Keyboard";
import { ScoreTile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";

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
      <div style={{ marginBottom: "auto" }}>
        <Phrase phrase={phrase} usedLetters={usedLetters} />
        <p
          style={{ color: "red" }}
        >{`Incorrect: ${incorrectLetters.join(", ")}`}</p>
      </div>
      {yourTurn ? (
        <>
          <Keyboard onKeyPress={onSelect} />
          <Timer startSeconds={30} />
        </>
      ) : null}
    </>
  );
};

export default Component;
