import { GameStatus } from "quiz-common-ui";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useEffect } from "react";
import { useParams } from "react-router";

const MiniGameStarting = () => {
  const { sendSync } = useLocalSync();
  const { gameId } = useParams<{ gameId: string }>();

  useEffect(() => {
    const timer = setTimeout(() => {
      sendSync("GameStatusUpdate", {
        gameId: gameId!,
        status: GameStatus.MiniGameStarted,
      });
    }, 10_000);

    return () => clearTimeout(timer);
  }, [gameId, sendSync]);

  return (
    <>
      <h1>MiniGameStarting</h1>
    </>
  );
};

export default MiniGameStarting;
