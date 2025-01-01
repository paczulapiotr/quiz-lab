import TutorialVideo from "../temp/TutorialVideo";
import { useParams } from "react-router";
import { useEffect } from "react";
import { PageTemplate } from "@repo/ui/components";
import { useLocalSync } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui/services/types";

const MiniGameStarting = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        sendSync("GameStatusUpdate", {
          gameId: gameId!,
          status: GameStatus.MiniGameStarted,
        }),
      10_000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, sendSync]);

  return (
    <PageTemplate>
      <TutorialVideo title="Zasady rundy..." />
    </PageTemplate>
  );
};

export default MiniGameStarting;
