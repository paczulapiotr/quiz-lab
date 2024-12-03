import { useGetGame } from "@/api/queries/useGetGame";
import { PageTemplate } from "@/components/PageTemplate";
import { Tile } from "@/components/Tile";
import { useState } from "react";
import { useParams } from "react-router";
import { useLocalSyncConsumer } from "@/hooks/useLocalSyncConsumer";

const JoinGame = () => {
  const [slots, setSlots] = useState<string[]>([]);
  const { gameId } = useParams<{ gameId: string }>();

  const { data, isLoading } = useGetGame(gameId);

  useLocalSyncConsumer("PlayerJoined", (payload) => {
    setSlots((prev) =>
      payload?.PlayerName ? [...prev, payload.PlayerName] : prev,
    );
  });

  return (
    <PageTemplate>
      <Tile blue text={"Waiting for other players..."} />
      {isLoading ? null : (
        <>
          <Tile text={`Slots ${slots.length}/${data?.gameSize ?? 0}`} />
          {slots.map((player, index) => (
            <Tile key={`${player}_${index}`} text={`${index + 1}. ${player}`} />
          ))}
        </>
      )}
    </PageTemplate>
  );
};

export default JoinGame;
