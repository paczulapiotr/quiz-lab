import { PageTemplate } from "@repo/ui/components";
import { useLocalSync } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui/services/types";
import TutorialVideo from "../temp/TutorialVideo";
import { useEffect } from "react";
import { useParams } from "react-router";

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
