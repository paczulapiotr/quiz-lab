import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Tile, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";
import { SorterActions } from "@repo/ui/minigames/actions";
import { useCallback } from "react";

type Props = {
  started?: boolean;
};

const Component = ({ started }: Props) => {
  const { mutate } = useUpdateMiniGame();
  const { gameId } = useGame();
  const { miniGameDefinition: definition, miniGameState: state } = useGame<
    SorterState,
    SorterDefinition
  >();
  const roundDef = definition?.rounds.find(
    (x) => x.id == state?.currentRoundId,
  );
  const left = roundDef?.leftCategory.name;
  const right = roundDef?.rightCategory.name;

  const onStart = useCallback(() => {
    mutate({
      gameId: gameId!,
      action: SorterActions.RoundStarted,
    });
  }, [gameId, mutate]);

  return (
    <>
      <Tile blue text={`Posortuj, ${left} czy ${right}?`} />;
      <div style={{ flex: 1 }} />
      <Timer
        onTimeUp={started ? undefined : onStart}
        startSeconds={
          started
            ? Times.Sorter.AnswerSeconds
            : Times.Sorter.RoundStartWaitSeconds
        }
      />
    </>
  );
};

export default Component;
