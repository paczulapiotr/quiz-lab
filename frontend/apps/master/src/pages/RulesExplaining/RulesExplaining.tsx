import { PageTemplate } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import TutorialVideo from "../temp/TutorialVideo";
import { useEffect } from "react";
import { useParams } from "react-router";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";

const RulesExplaining = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateGameStatus();

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
