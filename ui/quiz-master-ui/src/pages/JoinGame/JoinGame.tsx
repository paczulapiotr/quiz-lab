import { useGetGame } from "@/api/queries/useGetGame";
import { PageTemplate } from "@/components/PageTemplate";
import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { useCallback } from "react";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile, Timer } from "quiz-common-ui/components";
import styles from "./JoinGame.module.scss";

type Props = {
  starting?: boolean;
};

const JoinGame = ({ starting = false }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();

  const { data, isLoading, refetch } = useGetGame(gameId);

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "JoinGame",
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
    <PageTemplate>
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
