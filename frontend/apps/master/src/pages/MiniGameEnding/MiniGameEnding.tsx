import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import styles from "./MiniGameEnding.module.scss";
import { useEffect } from "react";
import { PageTemplate, HeaderTile, Tile } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

const MiniGameEnding = () => {
  const { gameId } = useGame();
  const { data } = useGetScore(gameId);
  const { mutate } = useUpdateGameStatus();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          status: GameStatus.MiniGameEnded,
        }),
      Times.GameEndingSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

  return (
    <PageTemplate squares>
      <HeaderTile title="Podsumowanie rundy" />
      <div className={styles.grid}>
        {data?.playerScores.map((player) => (
          <Tile
            text={`${player.playerName} - ${player.totalScore}`}
            key={player.playerDeviceId}
          />
        ))}
      </div>
    </PageTemplate>
  );
};

export default MiniGameEnding;
