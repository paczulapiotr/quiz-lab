import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import TutorialVideo from "@/pages/temp/TutorialVideo";
import Times from "@repo/ui/config/times";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useEffect } from "react";
import { useParams } from "react-router";

const Component = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          action: AbcdActions.PowerPlayExplainStop,
        }),
      Times.TEMP.PowerPlayExplainSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return <TutorialVideo title="Prezentacja zagrywek..." />;
};

export default Component;
