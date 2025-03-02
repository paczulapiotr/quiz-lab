import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { FamilyFeudInteractions } from "@repo/ui/minigames/actions";
import { useCallback } from "react";
import { useGame } from "@repo/ui/contexts/GameContext";

const AnswerQuestion = () => {
  const { mutateAsync } = useSendPlayerInteraction();
  const { gameId } = useGame();
  
  const onAnswer = useCallback(
    (ans: string) =>
      mutateAsync({
        gameId: gameId!,
        interactionType: FamilyFeudInteractions.Answer,
        value: ans,
      }),
    [gameId, mutateAsync],
  );

  return <Component onAnswer={onAnswer} />;
};

export default AnswerQuestion;
