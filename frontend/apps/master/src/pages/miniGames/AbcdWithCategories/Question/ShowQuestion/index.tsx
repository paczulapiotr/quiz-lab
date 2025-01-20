import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";

type Props = {
  gameId: string;
};

const ShowQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId!, true);

  return (
    <Component
      audioUrl={question.data?.audioUrl ?? ""}
      question={question.data?.question ?? ""}
      questionId={question.data?.questionId ?? ""}
    />
  );
};

export default ShowQuestion;
