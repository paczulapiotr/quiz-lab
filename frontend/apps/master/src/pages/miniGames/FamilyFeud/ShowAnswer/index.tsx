import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import { useEffect } from "react";

const ShowAnswer = () => {
  const { gameId } = useGame();
  const { answers, question, lastWrongAnswer } = useBoardItems(true);
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          action: FamilyFeudActions.AnswerShown,
        }),
      Times.FamilyFeud.AnswerShowSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return (
    <MainBoard
      answers={answers}
      question={question}
      wrongAnswer={lastWrongAnswer}
    />
  );
};

export default ShowAnswer;
