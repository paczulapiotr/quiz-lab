import { useCallback, useState } from "react";
import styles from "./StandardQuestionPage.module.scss";
import { PageTemplate } from "@/components/PageTemplate";
// import { useGetMiniGame } from "@/api/queries/useGetMiniGame";
// import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile } from "quiz-common-ui/components";

const StandardQuestionPage = () => {
  const { sendSync } = useLocalSyncService();
  const [selected, setSelected] = useState("");
  // const { gameId } = useParams<{ gameId: string }>();
  // const { data, refetch } = useGetMiniGame(gameId);

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "StandardQuestionPage",
    useCallback((message) => {
      if (message?.status === GameStatus.RoundStarting) {
        // refetch();
      }
    }, []),
  );

  useLocalSyncConsumer(
    "SelectAnswer",
    "StandardQuestionPage",
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
function useLocalSyncService(): { sendSync: any } {
  throw new Error("Function not implemented.");
}
