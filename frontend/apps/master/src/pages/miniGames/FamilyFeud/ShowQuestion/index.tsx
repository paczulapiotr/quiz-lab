import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import { useEffect } from "react";

const ShowQuestion = () => {
  const { gameId } = useGame();
  const { answers, question } = useBoardItems();
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          action: FamilyFeudActions.QuestionShown,
        }),
      Times.FamilyFeud.QuestionShowSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return <MainBoard answers={answers} question={question} />;
};

export default ShowQuestion;
