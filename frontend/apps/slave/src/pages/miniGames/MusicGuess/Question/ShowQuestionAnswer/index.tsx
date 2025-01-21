import Component from "./Component";
import { useGetQuestionAnswer } from "@/api/queries/minigames/music/useGetQuestionAnswer";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetQuestionAnswer(gameId, true);

  return (
    <Component
      answerId={data?.answer?.answerId}
      answerScore={data?.answer?.answerPoints ?? 0}
      score={data?.answer?.roundPoints ?? 0}
      answers={data?.answers ?? []}
    />
  );
};

export default ShowQuestionAnswer;
