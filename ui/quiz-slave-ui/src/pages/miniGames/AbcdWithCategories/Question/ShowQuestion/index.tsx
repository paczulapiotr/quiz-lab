import { useGetScore } from "@/api/queries/useGetScore";
import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";

type Props = {
  gameId: string;
};

const ShowQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId!, true);
  const { data: score } = useGetScore(gameId!);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      question={question.data?.question ?? ""}
      questionId={question.data?.questionId ?? ""}
    />
  );
};

export default ShowQuestion;
