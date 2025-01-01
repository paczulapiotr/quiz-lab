import { useJoinGameMutation } from "@/api/mutations/useJoinGameMutation";
import { useGetGame } from "@/api/queries/useGetGame";
import { useCallback, useState } from "react";
import { useParams } from "react-router";
import styles from "./JoinGame.module.scss";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui";
import { PageTemplate, TileButton, Tile, Timer } from "@repo/ui/components";


type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const [playerName, setPlayerName] = useState("");
  const { gameId } = useParams<{ gameId: string }>();

  const { mutateAsync } = useJoinGameMutation();
  const { data, isLoading, refetch } = useGetGame(gameId);
  const joined = data?.yourDeviceId != null;
  const joinGame = async () => mutateAsync({ playerName, gameId: gameId! });

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

  const playerNames = [
    ...(data?.playerNames ?? []),
    ...(data ? Array(data.gameSize - data.playerNames.length).fill(null) : []),
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
              <Tile blue key={`${player}_${index}`} text={`${player}`} />
            ),
          )}
        </div>
      ) : null}
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer startSeconds={10} />
        </div>
      ) : null}
    </PageTemplate>
  );
};

export default JoinGame;
