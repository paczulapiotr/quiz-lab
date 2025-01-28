import TutorialVideo from "../temp/TutorialVideo";
import { useParams } from "react-router";
import { useEffect } from "react";
import { PageTemplate } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";

const MiniGameStarting = () => {
  const { gameId } = useParams<{ gameId: string }>();
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
      <TutorialVideo title="Zasady rundy..." />
    </PageTemplate>
  );
};

export default MiniGameStarting;
