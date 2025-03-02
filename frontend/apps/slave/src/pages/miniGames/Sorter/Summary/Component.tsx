import { Tile } from "@repo/ui/components";
import styles from "./Component.module.scss";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { useMemo } from "react";
import { useGame } from "@repo/ui/contexts/GameContext";

const Component = () => {
  const {
    miniGameDefinition: definition,
    miniGameState: state,
    you,
  } = useGame<SorterState, SorterDefinition>();

  const { total, correct } = useMemo(() => {
    const roundDef = definition?.rounds.find(
      (x) => x.id == state?.currentRoundId,
    );
    const t =
      (roundDef?.leftCategory.items.length ?? 0) +
      (roundDef?.rightCategory.items.length ?? 0);
    const c =
      state?.rounds
        .find((x) => x.roundId == state?.currentRoundId)
        ?.answers.find((x) => x.playerId == you?.id)?.correctAnswers ?? 0;

    return { total: t, correct: c };
  }, [definition?.rounds, state?.currentRoundId, state?.rounds, you?.id]);

  return (
    <div className={styles.summary}>
      <Tile blue text={`Wynik rundy: ${correct} / ${total}`} />
    </div>
  );
};

export default Component;
