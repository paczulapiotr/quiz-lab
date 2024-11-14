import { ScoreTile } from "@/components/ScoreTile";
import { Tile } from "@/components/Tile";
import { Timer } from "@/components/Timer";
import styles from "./StandardQuestionPage.module.scss";
const StandardQuestionPage = () => {
  return (
    <div>
      <ScoreTile />
      <br />
      <Tile text="What is the capital of France?" blue />
      <div className={styles.answers}>
        <Tile text="What is the capital of France?" />
        <Tile text="What is the capital of France?" />
        <Tile text="What is the capital of France?" />
        <Tile text="What is the capital of France?" />
      </div>
      <Timer startSeconds={10} />
    </div>
  );
};

export default StandardQuestionPage;
