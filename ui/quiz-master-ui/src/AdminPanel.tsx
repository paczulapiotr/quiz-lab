import { GameStatus, GameStatusNames } from "quiz-common-ui";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useParams } from "react-router";

const AdminPanel = () => {
  const data = useParams<{ gameId: string }>();
  return (
    <>
      <p>{JSON.stringify(data)}</p>
      <p>GAME UPDATES</p>
      <div
        style={{
          width: "100%",
          display: "flex",
          gap: "1rem",
          paddingBottom: "1rem",
        }}
      >
        <SendGameUpdateButton status={GameStatus.RulesExplained} />
        <SendGameUpdateButton status={GameStatus.MiniGameStarted} />
        <SendGameUpdateButton status={GameStatus.MiniGameEnded} />
        <SendGameUpdateButton status={GameStatus.GameEnded} />
      </div>
      <p>MINI GAME UPDATES</p>
      <div style={{ width: "100%", display: "flex", gap: "1rem" }}>
        <SendMiniGameUpdateButton action="PowerPlayExplainStop" />
        <SendMiniGameUpdateButton action="PowerPlayApplyStop" />
        <SendMiniGameUpdateButton action="CategoryShowStop" />
        <SendMiniGameUpdateButton action="QuestionShowStop" />
        <SendMiniGameUpdateButton action="QuestionAnswerShowStop" />
      </div>
    </>
  );
};

export default AdminPanel;

const SendMiniGameUpdateButton = ({ action }: { action: string }) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();

  return (
    <button
      style={{ fontSize: "1rem", flexWrap: "wrap" }}
      onClick={() =>
        sendSync("MiniGameUpdate", {
          gameId: gameId!,
          action,
          miniGameType: "AbcdWithCategories",
        })
      }
    >
      {action}
    </button>
  );
};

const SendGameUpdateButton = ({ status }: { status: GameStatus }) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();

  return (
    <button
      style={{ fontSize: "1rem", flexWrap: "wrap" }}
      onClick={() =>
        sendSync("GameStatusUpdate", {
          gameId: gameId!,
          status,
        })
      }
    >
      {GameStatusNames[status]}
    </button>
  );
};
