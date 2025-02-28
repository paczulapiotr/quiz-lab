import { PageTemplate } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import TutorialVideo from "../temp/TutorialVideo";
import { useEffect } from "react";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

const RulesExplaining = () => {
  const { mutate } = useUpdateGameStatus();
  const { gameId } = useGame();
  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          status: GameStatus.RulesExplained,
        }),
      Times.TEMP.RulesExplainSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return (
    <PageTemplate>
      <TutorialVideo title="Zasady gry..." />;
    </PageTemplate>
  );
};

export default RulesExplaining;
