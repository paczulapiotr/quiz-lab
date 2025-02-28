import { useGetGame } from "@repo/ui/api/queries/useGetGame";
import { GameStatus } from "@repo/ui/services/types";
import styles from "./JoinGame.module.scss";
import { PageTemplate, Tile, Timer } from "@repo/ui/components";
import { useUpdateGameStatus } from "@repo/ui/api/mutations/useUpdateGameStatus";
import Times from "@repo/ui/config/times";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const { mutate } = useUpdateGameStatus();
  const { gameId, players } = useGame();
  const { data, isLoading } = useGetGame(gameId);

  const onGameStarted = () =>
    mutate({
      gameId: gameId!,
      status: GameStatus.GameStarted,
    });

  const spots: { id: string; name: string }[] = [
    ...(players ?? []),
    ...(data ? Array(data.gameSize - data.players.length).fill(null) : []),
  ];
  return (
    <PageTemplate squares>
      <p className={styles.waitForPlayers}>
        {starting ? "Rozpoczynanie gry" : "Czekanie na graczy"}
      </p>
      {isLoading ? null : (
        <div className={styles.grid}>
          {spots.map((player, index) =>
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
