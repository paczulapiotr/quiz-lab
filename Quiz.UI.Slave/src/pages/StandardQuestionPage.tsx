import { useEffect, useState } from "react";
import styles from "./StandardQuestionPage.module.scss";
import { PageTemplate } from "@/components/PageTemplate";
import { useGetMiniGame } from "@/api/queries/useGetMiniGame";
import { useParams } from "react-router";
import {
  GameStatus,
  Tile,
  useLocalSyncConsumer,
  useLocalSyncService,
} from "quiz-common-ui";

const StandardQuestionPage = () => {
  const { sendSync, onSync, offSync } = useLocalSyncService();
  const [selected, setSelected] = useState("");
  const { gameId } = useParams<{ gameId: string }>();
  const { data, refetch } = useGetMiniGame(gameId);

  console.log(data);
  useLocalSyncConsumer("GameStatusUpdate", (message) => {
    if (message?.status === GameStatus.RoundStarting) {
      refetch();
    }
  });

  useEffect(() => {
    onSync("SelectAnswer", (data) => {
      setSelected(data?.answer === selected ? "" : data!.answer);
    });
    return () => {
      offSync("SelectAnswer");
    };
  }, [offSync, onSync, selected]);

  const selectAnswer = async (answer: string) => {
    setSelected(answer === selected ? "" : answer);
    await sendSync("SelectAnswer", { answer });
  };

  return (
    <PageTemplate initialTimerSeconds={10} score={0}>
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
