import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetQuestion } from "@/api/queries/minigames/abcd/useGetQuestion";
import { useGetScore } from "@/api/queries/useGetScore";
import { useGetAppliedPowerPlay } from "@/api/queries/minigames/abcd/useGetAppliedPowerPlay";
import { useMemo } from "react";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const question = useGetQuestion(gameId, true);
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();
  const appliedPowerPlays = useGetAppliedPowerPlay(gameId, true);

  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: "QuestionAnswer", value });

  const powerPlays = useMemo(
    () =>
      appliedPowerPlays.data?.players[0]?.powerPlays.map((x) => x.powerPlay) ??
      [],
    [appliedPowerPlays.data?.players],
  );

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question.data?.answers ?? []}
      question={question.data?.question ?? ""}
      onAnswer={answer}
      powerPlays={powerPlays}
    />
  );
};

export default AnswerQuestion;
