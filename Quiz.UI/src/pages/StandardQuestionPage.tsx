import { ScoreTile } from "@/components/ScoreTile";
import { Tile } from "@/components/Tile";
import { Timer } from "@/components/Timer";
import styles from "./StandardQuestionPage.module.scss";
import { useLocalSyncService } from "@/hooks/useLocalSyncService";
import { useEffect } from "react";

const StandardQuestionPage = () => {
  const { connected, sendSync, onSync, offSync } = useLocalSyncService();

  useEffect(() => {
    onSync("Pong", (data) => {
      alert("Pong: " + JSON.stringify(data));
    });
    return () => {
      offSync("Pong");
    };
  }, [offSync, onSync]);

  return (
    <div>
      <ScoreTile />
      <br />
      <Tile text="What is the capital of France?" blue />
      <div className={styles.answers}>
        <Tile text="What is the capital of France?" selected />
        <Tile text="What is the capital of France?" success />
        <Tile text="What is the capital of France?" failure />
        <Tile text="What is the capital of France?" />
      </div>
      <Timer startSeconds={10} />
      <p>{connected ? "✅ Connected" : "❌ Disconnected"}</p>
      <button
        onClick={() => {
          sendSync("Ping", { Amount: 10, Message: "10 jabłek" });
        }}
      >
        {"Ping"}
      </button>
    </div>
  );
};

export default StandardQuestionPage;
