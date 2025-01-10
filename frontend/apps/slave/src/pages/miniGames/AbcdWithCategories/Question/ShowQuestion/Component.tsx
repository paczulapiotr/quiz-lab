import { ScoreTile, Tile, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";

type Props = {
  questionId: string;
  question: string;
  score: number;
};

const Component = ({ question, score }: Props) => {
  return (
    <>
      <ScoreTile score={score} />
      <Tile blue text={question} />
      <Timer startSeconds={Times.Abdc.QuestionShowSeconds} />
    </>
  );
};

export default Component;
