import { Tile, Timer } from "@repo/ui/components";
import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import styles from "./index.module.scss";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

const AnswerQuestion = () => {
  const { players } = useGame();
  const { answers, question, currentPlayerId } = useBoardItems();
  const currentPlayer = players.find((player) => player.id === currentPlayerId);

  return (
    <div className={styles.container}>
      <MainBoard answers={answers} question={question} />
      <Tile
        text={`ODPOWIADA: ${currentPlayer?.name}`}
        blue
        className={styles.player}
      />
      <Timer startSeconds={Times.FamilyFeud.AnswerSeconds} />
    </div>
  );
};

export default AnswerQuestion;
