import { useGetGame } from "@/api/queries/useGetGame";
import { useParams } from "react-router";
import { useCallback } from "react";
import { useLocalSyncConsumer} from "@repo/ui/hooks"
import { GameStatus } from "@repo/ui/services/types";
import styles from "./JoinGame.module.scss";
import { PageTemplate, Tile, Timer } from "@repo/ui/components";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();

  const { data, isLoading, refetch } = useGetGame(gameId);

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

  const players = [
    ...(data?.playerNames ?? []),
    ...(data ? Array(data.gameSize - data.playerNames.length).fill(null) : []),
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
              <Tile blue key={`${player}_${index}`} text={`${player}`} />
            ),
          )}
        </div>
      )}
      {starting ? (
        <div style={{ marginTop: "auto" }}>
          <Timer startSeconds={10} />
        </div>
      ) : null}
    </PageTemplate>
  );
};

export default JoinGame;
