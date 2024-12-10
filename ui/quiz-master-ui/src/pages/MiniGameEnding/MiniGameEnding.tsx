import { GameStatus } from "quiz-common-ui";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useEffect } from "react";
import { useParams } from "react-router";

const MiniGameEnding = () => {
  const { sendSync } = useLocalSync();
  const { gameId } = useParams<{ gameId: string }>();

  useEffect(() => {
    const timer = setTimeout(() => {
      sendSync("GameStatusUpdate", {
        gameId: gameId!,
        status: GameStatus.MiniGameEnded,
      });
    }, 10_000);

    return () => clearTimeout(timer);
  }, [gameId, sendSync]);

  return (
    <>
      <h1>MiniGameEnding</h1>
    </>
  );
};

export default MiniGameEnding;
