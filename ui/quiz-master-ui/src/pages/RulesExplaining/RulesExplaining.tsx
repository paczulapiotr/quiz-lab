import { GameStatus } from "quiz-common-ui";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useEffect } from "react";
import { useParams } from "react-router";

const RulesExplaining = () => {
  const { sendSync } = useLocalSync();
  const { gameId } = useParams<{ gameId: string }>();

  useEffect(() => {
    const timer = setTimeout(() => {
      sendSync("GameStatusUpdate", {
        gameId: gameId!,
        status: GameStatus.RulesExplained,
      });
    }, 10_000);

    return () => clearTimeout(timer);
  }, [gameId, sendSync]);

  return (
    <>
      <h1>RulesExplaining</h1>
    </>
  );
};

export default RulesExplaining;
