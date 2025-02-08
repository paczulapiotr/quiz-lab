import { useGetGame } from "@repo/ui/api/queries/useGetGame";
import { useParams } from "react-router";
import { useCallback, useEffect } from "react";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui/services/types";
import styles from "./JoinGame.module.scss";
import { PageTemplate, Tile, Timer } from "@repo/ui/components";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateGameStatus();
  const { data, isLoading, refetch } = useGetGame(gameId);
  const { setPlayers } = usePlayers();

  useEffect(() => {
    setPlayers(data?.players ?? []);
  }, [data?.players, setPlayers]);

  const onGameStarted = () =>
    mutate({
      gameId: gameId!,
      status: GameStatus.GameStarted,
    });

  useLocalSyncConsumer(
    "GameStatusUpdate",
    useCallback(
      (payload) => {
        if (payload?.status === GameStatus.GameJoined) {
          refetch();
        }
      },
      [refetch],
    ),
  );

  const players: { id: string; name: string }[] = [
    ...(data?.players ?? []),
    ...(data ? Array(data.gameSize - data.players.length).fill(null) : []),
  ];
  return (
    <PageTemplate squares>
      <p className={styles.waitForPlayers}>
        {starting ? "Rozpoczynanie gry" : "Czekanie na graczy"}
      </p>
      {isLoading ? null : (
        <div className={styles.grid}>
          {players.map((player, index) =>
            player == null ? (
              <Tile text="..." key={index} className={styles.emptySpot} />
            ) : (
              <Tile blue key={`${player?.id}_${index}`} text={player.name} />
            ),
          )}
        </div>
      )}
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer
            startSeconds={Times.GameStartingSeconds}
            onTimeUp={onGameStarted}
          />
        </div>
      ) : null}
    </PageTemplate>
  );
};

export default JoinGame;
