import Component from "./Component";
import { useGetQuestionAnswer } from "@/api/queries/minigames/abcd/useGetQuestionAnswer";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetQuestionAnswer(gameId, true);

  return (
    <Component answers={data?.answers ?? []} players={data?.players ?? []} />
  );
};

export default ShowQuestionAnswer;
