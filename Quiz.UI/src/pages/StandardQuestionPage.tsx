import { Tile } from "@/components/Tile";
import { useLocalSyncService } from "@/hooks/useLocalSyncService";
import { useEffect, useState } from "react";
import styles from "./StandardQuestionPage.module.scss";
import { PageTemplate } from "@/components/PageTemplate";

const StandardQuestionPage = () => {
  const { sendSync, onSync, offSync } = useLocalSyncService();
  const [selected, setSelected] = useState("");

  useEffect(() => {
    onSync("SelectAnswer", (data) => {
      setSelected(data?.Answer === selected ? "" : data!.Answer);
    });
    return () => {
      offSync("SelectAnswer");
    };
  }, [offSync, onSync, selected]);

  const selectAnswer = async (answer: string) => {
    setSelected(answer === selected ? "" : answer);
    await sendSync("SelectAnswer", { Answer: answer });
  };

  return (
    <PageTemplate initialTimerSeconds={10}>
      <div className={styles.question}>
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
      </div>
    </PageTemplate>
  );
};

export default StandardQuestionPage;
