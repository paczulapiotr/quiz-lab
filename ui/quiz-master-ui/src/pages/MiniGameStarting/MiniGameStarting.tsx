import TutorialVideo from "../temp/TutorialVideo";
import { GameStatus } from "quiz-common-ui";
import { useParams } from "react-router";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useEffect } from "react";
import { PageTemplate } from "quiz-common-ui/components";

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
