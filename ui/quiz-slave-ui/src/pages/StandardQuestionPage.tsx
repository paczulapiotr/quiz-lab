import { useCallback, useState } from "react";
import styles from "./StandardQuestionPage.module.scss";
// import { useGetMiniGame } from "@/api/queries/useGetMiniGame";
// import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { useLocalSync, useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { PageTemplate, Tile } from "quiz-common-ui/components";

const StandardQuestionPage = () => {
  const { sendSync } = useLocalSync();
  const [selected, setSelected] = useState("");
  // const { gameId } = useParams<{ gameId: string }>();
  // const { data, refetch } = useGetMiniGame(gameId);

  useLocalSyncConsumer(
    "GameStatusUpdate",
    useCallback((message) => {
      if (message?.status === GameStatus.MiniGameStarting) {
        // refetch();
      }
    }, []),
  );

  useLocalSyncConsumer(
    "SelectAnswer",
    useCallback(
      (data) => {
        setSelected(data?.answer === selected ? "" : data!.answer);
      },
      [selected],
    ),
  );

  const selectAnswer = async (answer: string) => {
    setSelected(answer === selected ? "" : answer);
    await sendSync("SelectAnswer", { answer });
  };
  console.log(selectAnswer);

  return (
    <PageTemplate initialTimerSeconds={10}>
      <div className={styles.question}>
        <Tile text="What is the capital of France?" blue />
        <div className={styles.answers}>
          <Tile
            text="What is the capital of France?"
            selected={selected === "A"}
          />
          <Tile
            text="What is the capital of France?"
            selected={selected === "B"}
          />
          <Tile
            text="What is the capital of France?"
            selected={selected === "C"}
          />
          <Tile
            text="What is the capital of France?"
            selected={selected === "D"}
          />
        </div>
      </div>
    </PageTemplate>
  );
};

export default StandardQuestionPage;
