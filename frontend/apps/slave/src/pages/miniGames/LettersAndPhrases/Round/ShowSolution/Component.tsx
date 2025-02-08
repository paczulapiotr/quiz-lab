import { ScoreTile, Timer } from "@repo/ui/components";
import { Phrase } from "@repo/ui/components/minigames/LettersAndPhrases/Phrase";
import Times from "@repo/ui/config/times";

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
      <Timer startSeconds={Times.Letters.ShowResolvedPhraseSeconds} />
    </>
  );
};

export default Component;
