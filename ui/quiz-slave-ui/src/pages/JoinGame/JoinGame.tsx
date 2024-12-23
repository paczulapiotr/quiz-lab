import { useJoinGameMutation } from "@/api/mutations/useJoinGameMutation";
import { useGetGame } from "@/api/queries/useGetGame";
import { PageTemplate } from "@/components/PageTemplate";
import { useCallback, useState } from "react";
import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import styles from "./JoinGame.module.scss";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile, TileButton, Timer } from "quiz-common-ui/components";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const [joined, setJoined] = useState(false);
  const [playerName, setPlayerName] = useState("");
  const { gameId } = useParams<{ gameId: string }>();

  const { mutateAsync } = useJoinGameMutation();
  const { data, isLoading, refetch } = useGetGame(gameId);

  const joinGame = async () =>
    mutateAsync({ playerName, gameId: gameId! }).then(() => setJoined(true));

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "JoinGame",
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

  console.log(playerNames);

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
