import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId, true);

  const { mutateAsync: sendAsync } = useSendPlayerInteraction();
  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: "QuestionAnswer", value });

  return (
    <Component
      answers={question.data?.answers ?? []}
      question={question.data?.question ?? ""}
      onAnswer={answer}
    />
  );
};

export default AnswerQuestion;
