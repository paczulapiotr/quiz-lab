import { useLocalSync } from "quiz-common-ui/hooks";
import { useState } from "react";

const AdminPanel = () => {
  const [gameId, setGameId] = useState<string>("");
  const [action, setAction] = useState<string>("");

  const { sendSync } = useLocalSync();
  return (
    <div style={{ width: "100%", display: "flex" }}>
      <label htmlFor="gameId">Game ID</label>
      <input
        id="gameId"
        value={gameId}
        onChange={(e) => setGameId(e.target.value)}
      />
      <label htmlFor="action">Action</label>
      <input
        id="action"
        value={action}
        onChange={(e) => setAction(e.target.value)}
      />

      <button
        onClick={() =>
          sendSync("MiniGameUpdate", {
            gameId: gameId,
            action: action,
            miniGameType: "AbcdWithCategories",
          })
        }
      >
        {"Send MiniGameUpdate"}
      </button>
    </div>
  );
};

export default AdminPanel;
