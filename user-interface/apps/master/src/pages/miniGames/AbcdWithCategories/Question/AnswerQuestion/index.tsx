import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId, true);

  return (
    <Component
      answers={question.data?.answers ?? []}
      question={question.data?.question ?? ""}
    />
  );
};

export default AnswerQuestion;
