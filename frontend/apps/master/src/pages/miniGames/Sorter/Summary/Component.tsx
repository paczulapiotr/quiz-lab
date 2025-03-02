import styles from "./Component.module.scss";
import {
  SorterDefinition,
  SorterState,
} from "@repo/ui/api/queries/minigames/sorter";
import { useMemo } from "react";
import { Tile } from "@repo/ui/components";
import { useGame } from "@repo/ui/contexts/GameContext";

const Component = () => {
  const { miniGameState: state, miniGameDefinition: definition, players } = useGame<SorterState, SorterDefinition>();
  const playerScores = useMemo(() => {
    const roundDef = definition?.rounds.find(
      (x) => x.id == state?.currentRoundId,
    );

    const t =
      (roundDef?.leftCategory.items.length ?? 0) +
      (roundDef?.rightCategory.items.length ?? 0);

    const round = state?.rounds.find(
      (x) => x.roundId == state?.currentRoundId,
    );

    return players.map((p) => ({
      playerId: p.id,
      playerName: p.name,
      total: t,
      correct:
        round?.answers.find((x) => x.playerId === p.id)?.correctAnswers ?? 0,
    }));
  }, [
    definition?.rounds,
    state?.currentRoundId,
    state?.rounds,
    players,
  ]);

  return (
    <>
      <Tile blue text="Wyniki rundy:" />
      <div className={styles.grid}>
        {playerScores.map((p) => (
          <Tile
            key={p.playerId}
            blue
            text={`${p.playerName} - ${p.correct}/${p.total}`}
          />
        ))}
      </div>
    </>
  );
};

export default Component;
