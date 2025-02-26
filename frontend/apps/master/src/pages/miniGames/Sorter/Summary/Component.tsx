import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import styles from "./Component.module.scss";
import {
  SorterDefinition,
  SorterState,
} from "@repo/ui/api/queries/minigames/sorter";
import { useMemo } from "react";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";
import { Tile } from "@repo/ui/components";

type Props = {
  gameId?: string;
};

const Component = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<SorterState, SorterDefinition>(gameId);
  const { players } = usePlayers();
  const playerScores = useMemo(() => {
    const roundDef = data?.definition?.rounds.find(
      (x) => x.id == data.state?.currentRoundId,
    );

    const t =
      (roundDef?.leftCategory.items.length ?? 0) +
      (roundDef?.rightCategory.items.length ?? 0);

    const round = data?.state?.rounds.find(
      (x) => x.roundId == data?.state?.currentRoundId,
    );

    return players.map((p) => ({
      playerId: p.id,
      playerName: p.name,
      total: t,
      correct:
        round?.answers.find((x) => x.playerId === p.id)?.correctAnswers ?? 0,
    }));
  }, [
    data?.definition?.rounds,
    data?.state?.currentRoundId,
    data?.state?.rounds,
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
