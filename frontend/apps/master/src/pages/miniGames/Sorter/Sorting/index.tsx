import { useEffect } from "react";
import Component from "./Component";
import { SorterActions } from "@repo/ui/minigames/actions";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  started?: boolean;
};

const Sorting = ({ started }: Props) => {
  const { mutate } = useUpdateMiniGame();
  const { gameId } = useGame();

  useEffect(() => {
    if (!started) {
      setTimeout(
        () =>
          mutate({
            gameId: gameId!,
            action: SorterActions.RoundStarted,
          }),
        Times.Sorter.RoundStartWaitSeconds * 1_000,
      );
    }
  }, [gameId, mutate, started]);

  return <Component />;
};

export default Sorting;
