import { useJoinGameMutation } from "@/api/mutations/useJoinGameMutation";
import { useGetGame } from "@/api/queries/useGetGame";
import { PageTemplate } from "@/components/PageTemplate";
import { useCallback, useState } from "react";
import { useNavigate, useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile, Timer } from "quiz-common-ui/components";
import styles from "./JoinGame.module.scss";

const JoinGame = () => {
  const [joined, setJoined] = useState(false);
  const [starting, setStarting] = useState(false);
  const [playerName, setPlayerName] = useState("");
  const { gameId } = useParams<{ gameId: string }>();
  const navigate = useNavigate();

  const { mutateAsync } = useJoinGameMutation();
  const { data, isLoading } = useGetGame(gameId);

  const joinGame = async () =>
    mutateAsync({ playerName, gameId: gameId! }).then(() => setJoined(true));

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "JoinGame",
    useCallback(
      (message) => {
        switch (message?.status) {
          case GameStatus.GameStarting:
            setStarting(true);
            break;
          case GameStatus.GameStarted:
            navigate(`/${gameId}/question`);
            break;
          default:
            break;
        }
      },
      [gameId, navigate],
    ),
  );

  return (
    <PageTemplate>
      {joined ? (
        <Tile
          blue
          text={starting ? "Starting..." : "Waiting for other players..."}
        />
      ) : (
        <>
          <input
            className={styles.playerName}
            type="text"
            placeholder="Your name"
            value={playerName}
            onChange={(e) => setPlayerName(e.target.value)}
          />
          <Tile text={`Join Game`} blue onClick={joinGame} />
        </>
      )}
      {isLoading ? null : (
        <>
          <Tile
            text={`Slots ${data?.playerNames.length}/${data?.gameSize ?? 0}`}
          />
          {data?.playerNames.map((player, index) => (
            <Tile key={`${player}_${index}`} text={`${index + 1}. ${player}`} />
          ))}
        </>
      )}
      {starting ? null : <p onClick={() => navigate("/")}>Exit</p>}
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer startSeconds={10} />
        </div>
      ) : null}
    </PageTemplate>
  );
};

export default JoinGame;
