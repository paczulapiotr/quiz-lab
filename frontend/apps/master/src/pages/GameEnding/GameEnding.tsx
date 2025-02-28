import AdminPanel from "@/AdminPanel";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import styles from "./GameEnding.module.scss";
import { HeaderTile, Tile } from "@repo/ui/components";
import { useEffect } from "react";
import { useUpdateGameStatus } from "@repo/ui//api/mutations/useUpdateGameStatus";
import { GameStatus } from "@repo/ui";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  ended?: boolean;
};

const GameEnding = ({ ended }: Props) => {
  const { gameId } = useGame();
  const { data } = useGetScore(gameId);
  const { mutate } = useUpdateGameStatus();

  useEffect(() => {
    let timeout: NodeJS.Timeout | undefined;
    if (!ended) {
      timeout = setTimeout(() => {
        mutate({
          gameId: gameId!,
          status: GameStatus.GameEnded,
        });
      }, 10_000);
    }

    return () => {
      if (timeout) {
        clearTimeout(timeout);
      }
    };
  }, [ended, gameId, mutate]);

  return (
    <>
      <HeaderTile title="Podsumowanie gry" />
      <div className={styles.grid}>
        {data?.playerScores
          .sort((a, b) => b.totalScore - a.totalScore)
          .map((player, index) => (
            <Tile
              blue={index === 0}
              text={`${index + 1}. ${player.playerName} - ${player.totalScore}`}
              key={player.playerDeviceId}
            />
          ))}
      </div>
      <AdminPanel />
    </>
  );
};

export default GameEnding;
