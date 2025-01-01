import AdminPanel from "@/AdminPanel";
import { useGetScore } from "@/api/queries/useGetScore";
import { HeaderTile, PageTemplate, Tile } from "quiz-common-ui/components";
import { useParams } from "react-router";
import styles from "./MiniGameEnding.module.scss";
import { useLocalSync } from "quiz-common-ui/hooks";
import { GameStatus } from "quiz-common-ui";
import { useEffect } from "react";

const MiniGameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data } = useGetScore(gameId);
  const { sendSync } = useLocalSync();
  console.log(data);
  useEffect(() => {
    const timeout = setTimeout(
      () =>
        sendSync("GameStatusUpdate", {
          gameId: gameId!,
          status: GameStatus.MiniGameEnded,
        }),
      10_000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, sendSync]);

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
      <AdminPanel />
    </PageTemplate>
  );
};

export default MiniGameEnding;
