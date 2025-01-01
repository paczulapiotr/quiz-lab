import { ScoreTile, Tile, Timer } from "quiz-common-ui/components";

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
      <Timer startSeconds={9} />
    </>
  );
};

export default Component;
