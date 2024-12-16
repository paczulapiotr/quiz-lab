import { useState } from "react";
import instance from "./api/instance";

const AdminPanel = () => {
  const [gameId, setGameId] = useState<string>("");
  const [interactionType, setInteractionType] = useState<string>("");
  const [value, setValue] = useState<string>("");
  const [data, setData] = useState<string>("");

  return (
    <div style={{ width: "100%", display: "flex" }}>
      <div style={{ width: "100%", display: "flex", flexDirection:"column" }}>
      <label htmlFor="gameId">Game ID: </label>
      <input
        id="gameId"
        value={gameId}
        onChange={(e) => setGameId(e.target.value)}
      />
       <label htmlFor="interactionType">Interaction Type: </label>
      <input
        id="interactionType"
        value={interactionType}
        onChange={(e) => setInteractionType(e.target.value)}
      />
      <label htmlFor="value">Value: </label>
      <input
        id="value"
        value={value}
        onChange={(e) => setValue(e.target.value)}
        />
      </div>
        
      <label htmlFor="data">Data: </label>
      <textarea
        id="data"
        rows={10}
        style={{ resize: "none", width: "100%" }}
        value={data}
        onChange={(e) => setData(e.target.value)}
      />
      <button onClick={() => instance.post(`/game/${gameId}/mini-game/interaction`, {
        interactionType,
        value,
        data: JSON.parse(data),
      })}>
        {"Send PlayerInteraction"}
      </button>
    </div>
  );
};

export default AdminPanel;
