import TutorialVideo from "../temp/TutorialVideo";
import { useEffect } from "react";
import { PageTemplate } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

const MiniGameStarting = () => {
  const { gameId } = useGame();
  const { mutate } = useUpdateGameStatus();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          status: GameStatus.MiniGameStarted,
        }),
      Times.TEMP.MiniGameStartingSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return (
    <PageTemplate>
      <TutorialVideo />
    </PageTemplate>
  );
};

export default MiniGameStarting;
