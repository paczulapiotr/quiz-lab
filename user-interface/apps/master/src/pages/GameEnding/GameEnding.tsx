import AdminPanel from "@/AdminPanel";
import { useGetScore } from "@/api/queries/useGetScore";
import { useParams } from "react-router";
import styles from "./GameEnding.module.scss";
import { HeaderTile, Tile} from "@repo/ui/components"

const GameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data } = useGetScore(gameId);

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
