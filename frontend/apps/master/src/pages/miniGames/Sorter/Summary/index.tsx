import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Component from "./Component";
import { useEffect } from "react";
import { SorterActions } from "@repo/ui/minigames/actions";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  finished?: boolean;
};

const Summary = ({ finished }: Props) => {
  const { gameId} = useGame();
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    if (!finished) {
      setTimeout(
        () =>
          mutate({
            gameId: gameId!,
            action: SorterActions.RoundSummary,
          }),
        Times.Sorter.RoundStartWaitSeconds * 1000,
      );
    }
  }, [mutate, finished, gameId]);

  return <Component />;
};

export default Summary;
