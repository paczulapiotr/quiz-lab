import Component from "./Component";
import { useGetQuestionAnswer } from "@/api/queries/minigames/abcd/useGetQuestionAnswer";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const questionAnswer = useGetQuestionAnswer(gameId, true);
  return (
    <Component
      answerId={questionAnswer.data?.answerId ?? ""}
      answers={questionAnswer.data?.answers ?? []}
    />
  );
};

export default ShowQuestionAnswer;
