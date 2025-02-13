import { useEffect } from "react";
import Component from "./Component";
import { SorterActions } from "@repo/ui/minigames/actions";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";

type Props = {
  gameId?: string;
  started?: boolean;
};

const Sorting = ({ gameId, started }: Props) => {
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    if (!started) {
      debugger
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

  return <Component gameId={gameId} started={started} />;
};

export default Sorting;
