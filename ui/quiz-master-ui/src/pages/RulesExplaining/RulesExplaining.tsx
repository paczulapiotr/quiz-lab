import TutorialVideo from "../temp/TutorialVideo";
import { useEffect } from "react";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { PageTemplate } from "quiz-common-ui/components";

const RulesExplaining = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        sendSync("GameStatusUpdate", {
          gameId: gameId!,
          status: GameStatus.RulesExplained,
        }),
      10_000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, sendSync]);

  return (
    <PageTemplate>
      <TutorialVideo title="Zasady gry..." />;
    </PageTemplate>
  );
};

export default RulesExplaining;
