import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { FamilyFeudInteractions } from "@repo/ui/minigames/actions";
import { useCallback } from "react";

type Props = {
  gameId?: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const { mutateAsync } = useSendPlayerInteraction();

  const onAnswer = useCallback(
    (ans: string) =>
      mutateAsync({
        gameId: gameId!,
        interactionType: FamilyFeudInteractions.Answer,
        value: ans,
      }),
    [gameId, mutateAsync],
  );

  return <Component gameId={gameId} onAnswer={onAnswer} />;
};

export default AnswerQuestion;
