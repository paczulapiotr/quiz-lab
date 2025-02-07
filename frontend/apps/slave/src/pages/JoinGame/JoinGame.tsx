import { useJoinGameMutation } from "@repo/ui/api/mutations/useJoinGameMutation";
import { useGetGame } from "@repo/ui/api/queries/useGetGame";
import { useCallback, useState } from "react";
import { useParams } from "react-router";
import styles from "./JoinGame.module.scss";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui";
import { PageTemplate, TileButton, Tile, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import { Keyboard } from "@/components/Keyboard";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const [playerName, setPlayerName] = useState("");
  const { gameId } = useParams<{ gameId: string }>();

  const { mutateAsync } = useJoinGameMutation();
  const { data, isLoading, refetch } = useGetGame(gameId);
  const joined = data?.yourDeviceId != null;

  const joinGame = async () => {
    const resp = await mutateAsync({ playerName, gameId: gameId! });
    const { ok, errorCode } = resp;
    if (!ok) {
      if (errorCode === "NameAlreadyTaken") {
        alert("Nazwa jest już zajęta");
      }
    }
  };

  useLocalSyncConsumer(
    "GameStatusUpdate",
    useCallback(
      (message) => {
        switch (message?.status) {
          case GameStatus.GameJoined:
            refetch();
            break;
          default:
            break;
        }
      },
      [refetch],
    ),
  );

  const playerNames: { id: string; name: string }[] = [
    ...(data?.players ?? []),
    ...(data ? Array(data.gameSize - data.players.length).fill(null) : []),
  ];

  return (
    <PageTemplate>
      <div className={styles.header}>
        {joined ? (
          <p className={styles.waitForPlayers}>
            {starting ? "Rozpoczynanie gry" : "Czekanie na graczy"}
          </p>
        ) : (
          <div className={styles.input}>
            <input
              maxLength={40}
              className={styles.playerName}
              type="text"
              placeholder="Podaj nazwę gracza"
              value={playerName}
              onChange={(e) => setPlayerName(e.target.value)}
            />
            <TileButton
              text={`Dołącz`}
              blue
              onClick={joinGame}
              className={styles.button}
            />
          </div>
        )}
      </div>
      {!isLoading ? (
        <div className={styles.grid}>
          {playerNames.map((player, index) =>
            player == null ? (
              <Tile text="..." key={index} className={styles.emptySpot} />
            ) : (
              <Tile blue key={`${player?.id}_${index}`} text={player.name} />
            ),
          )}
        </div>
      ) : null}
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer startSeconds={Times.GameStartingSeconds} />
        </div>
      ) : (
        <div style={{ marginTop: "auto" }}>
          <Keyboard
            onChange={(e) => setPlayerName(e)}
            onKeyPress={console.log}
          />
        </div>
      )}
    </PageTemplate>
  );
};

export default JoinGame;
