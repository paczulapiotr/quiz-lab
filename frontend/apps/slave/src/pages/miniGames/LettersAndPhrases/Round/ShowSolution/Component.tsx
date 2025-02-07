import { ScoreTile } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";

type Props = {
  phrase: string[];
  score: number;
};

const Component = ({ score, phrase }: Props) => {
  return (
    <>
      <ScoreTile score={score} />
      <div style={{ marginBottom: "auto" }}>
        <Phrase phrase={phrase} solved />
      </div>
    </>
  );
};

export default Component;
