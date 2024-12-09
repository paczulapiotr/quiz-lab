import { useGetGame } from "@/api/queries/useGetGame";
import { PageTemplate } from "@/components/PageTemplate";
import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";
import { useCallback } from "react";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile } from "quiz-common-ui/components";

const JoinGame = () => {
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

  const players = data?.playerNames ?? [];

  return (
    <PageTemplate>
      <Tile blue text={"Waiting for other players..."} />
      {isLoading ? null : (
        <>
          <Tile text={`Slots ${players.length}/${data?.gameSize ?? 0}`} />
          {players.map((player, index) => (
            <Tile key={`${player}_${index}`} text={`${index + 1}. ${player}`} />
          ))}
        </>
      )}
    </PageTemplate>
  );
};

export default JoinGame;
