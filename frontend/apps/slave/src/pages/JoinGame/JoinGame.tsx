import { useJoinGameMutation } from "@repo/ui/api/mutations/useJoinGameMutation";
import { useState } from "react";
import styles from "./JoinGame.module.scss";
import { PageTemplate, Tile, Timer, TextInput } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import { Keyboard } from "@/components/Keyboard";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const [playerName, setPlayerName] = useState("");
  const { mutateAsync } = useJoinGameMutation();
  const { gameId, you, players } = useGame();
  const joined = you != null;

  const joinGame = async () => {
    const resp = await mutateAsync({ playerName, gameId: gameId! });
    const { ok, errorCode } = resp;
    if (!ok) {
      if (errorCode === "NameAlreadyTaken") {
        alert("Nazwa jest już zajęta");
      }
    }
  };

  return (
    <PageTemplate>
      <div className={styles.header}>
        {joined ? (
          <p className={styles.waitForPlayers}>
            {starting ? "Rozpoczynanie gry" : "Czekanie na graczy"}
          </p>
        ) : (
          <TextInput
            placeholder="Podaj nazwę gracza"
            buttonText="Dołącz"
            value={playerName}
            onChange={setPlayerName}
            onClick={joinGame}
          />
        )}
      </div>
      <div className={styles.grid}>
        {players.map((player, index) =>
          player == null ? (
            <Tile text="..." key={index} className={styles.emptySpot} />
          ) : (
            <Tile blue key={`${player?.id}_${index}`} text={player.name} />
          ),
        )}
      </div>
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer startSeconds={Times.GameStartingSeconds} />
        </div>
      ) : joined ? null : (
        <div style={{ marginTop: "auto" }}>
          <Keyboard value={playerName} onChange={setPlayerName} />
        </div>
      )}
    </PageTemplate>
  );
};

export default JoinGame;
