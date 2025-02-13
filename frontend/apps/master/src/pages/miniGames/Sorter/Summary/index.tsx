import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Component from "./Component";
import { useEffect } from "react";
import { SorterActions } from "@repo/ui/minigames/actions";
import Times from "@repo/ui/config/times";

type Props = {
  gameId?: string;
  finished?: boolean;
};

const Summary = ({ gameId,  finished }: Props) => {
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    if (!finished) {
      setTimeout(
        () =>
          mutate({
            gameId: gameId!,
            action: SorterActions.RoundSummary,
          }),
        Times.Sorter.RoundStartWaitSeconds,
      );
    }
  }, [gameId, mutate, finished]);

  return <Component gameId={gameId} />;
};

export default Summary;
