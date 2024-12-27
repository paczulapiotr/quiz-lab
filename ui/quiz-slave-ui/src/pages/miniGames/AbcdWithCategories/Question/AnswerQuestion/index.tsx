import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";
import { useGetScore } from "@/api/queries/useGetScore";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId, true);
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();
  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: "QuestionAnswer", value });

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question.data?.answers ?? []}
      question={question.data?.question ?? ""}
      onAnswer={answer}
    />
  );
};

export default AnswerQuestion;
