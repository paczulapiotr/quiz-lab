import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/music/useGetQuestion";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId, true);

  return (
    <Component
      answers={question.data?.answers ?? []}
      question={question.data?.question}
      questionAudio={question.data?.audioUrl}
    />
  );
};

export default AnswerQuestion;
