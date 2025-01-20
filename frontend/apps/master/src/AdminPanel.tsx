import { GameStatus, GameStatusNames } from "@repo/ui/services/types";
import { useParams } from "react-router";
import { useUpdateGameStatus } from "./api/mutations/useUpdateGameStatus";
import { useUpdateMiniGame } from "./api/mutations/useUpdateMiniGame";
import { AbcdActions } from "@repo/ui/minigames/actions";

const AdminPanel = () => {
  const { gameId } = useParams<{ gameId: string }>();
  return (
    <div
      style={{
        color: "white",
        marginTop: "auto",
        position: "absolute",
        top: "100%",
        zIndex: 1,
      }}
    >
      <p>{gameId}</p>
      <p>GAME UPDATES</p>
      <div
        style={{
          width: "100%",
          display: "flex",
          gap: "1rem",
          paddingBottom: "1rem",
        }}
      >
        <SendGameUpdateButton
          status={GameStatus.RulesExplained}
          gameId={gameId!}
        />
        <SendGameUpdateButton
          status={GameStatus.MiniGameStarted}
          gameId={gameId!}
        />
        <SendGameUpdateButton
          status={GameStatus.MiniGameEnded}
          gameId={gameId!}
        />

        <SendGameUpdateButton status={GameStatus.GameEnded} gameId={gameId!} />
      </div>
      <p>MINI GAME UPDATES</p>
      <div style={{ width: "100%", display: "flex", gap: "1rem" }}>
        <SendMiniGameUpdateButton
          action={AbcdActions.PowerPlayExplainStop}
          gameId={gameId!}
        />
        <SendMiniGameUpdateButton
          action={AbcdActions.PowerPlayApplyStop}
          gameId={gameId!}
        />
        <SendMiniGameUpdateButton action={AbcdActions.CategoryShowStop} gameId={gameId!} />
        <SendMiniGameUpdateButton action={AbcdActions.QuestionShowStop} gameId={gameId!} />
        <SendMiniGameUpdateButton
          action={AbcdActions.QuestionAnswerShowStop}
          gameId={gameId!}
        />
      </div>
    </div>
  );
};

export default AdminPanel;

const SendMiniGameUpdateButton = ({
  action,
  gameId,
}: {
  action: string;
  gameId: string;
}) => {
  const { mutate } = useUpdateMiniGame();

  return (
    <button
      style={{ fontSize: "1rem", flexWrap: "wrap" }}
      onClick={() =>
        mutate({
          gameId: gameId!,
          action,
        })
      }
    >
      {action}
    </button>
  );
};

const SendGameUpdateButton = ({
  status,
  gameId,
}: {
  status: GameStatus;
  gameId: string;
}) => {
  const { mutate } = useUpdateGameStatus();

  return (
    <button
      style={{ fontSize: "1rem", flexWrap: "wrap" }}
      onClick={() =>
        mutate({
          gameId: gameId!,
          status,
        })
      }
    >
      {GameStatusNames[status]}
    </button>
  );
};
