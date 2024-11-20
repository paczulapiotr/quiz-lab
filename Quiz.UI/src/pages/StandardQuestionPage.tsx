import { ScoreTile } from "@/components/ScoreTile";
import { Tile } from "@/components/Tile";
import { Timer } from "@/components/Timer";
import { useLocalSyncService } from "@/hooks/useLocalSyncService";
import { useEffect, useState } from "react";
import styles from "./StandardQuestionPage.module.scss";

const StandardQuestionPage = () => {
  const { connected, sendSync, onSync, offSync } = useLocalSyncService();
  const [selected, setSelected] = useState("");

  useEffect(() => {
    onSync("Pong", (data) => {
      alert("Pong: " + JSON.stringify(data));
    });
    onSync("SelectAnswer", (data) => {
      setSelected(data.Answer === selected ? "" : data.Answer);
    });
    return () => {
      offSync("Pong");
    };
  }, [offSync, onSync, selected]);

  const selectAnswer = async (answer: string) => {
    setSelected(answer === selected ? "" : answer);
    await sendSync("SelectAnswer", { Answer: answer });
  };

  return (
    <div className={styles.page}>
      <ScoreTile />
      <br />
      <Tile text="What is the capital of France?" blue />
      <div className={styles.answers}>
        <Tile
          text="What is the capital of France?"
          selected={selected === "A"}
          onClick={() => selectAnswer("A")}
        />
        <Tile
          text="What is the capital of France?"
          selected={selected === "B"}
          onClick={() => selectAnswer("B")}
        />
        <Tile
          text="What is the capital of France?"
          selected={selected === "C"}
          onClick={() => selectAnswer("C")}
        />
        <Tile
          text="What is the capital of France?"
          selected={selected === "D"}
          onClick={() => selectAnswer("D")}
        />
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
