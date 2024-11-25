import { PageTemplate } from "@/components/PageTemplate";
import { Tile } from "@/components/Tile";
import { useLocalSyncConsumer } from "@/hooks/useLocalSyncConsumer";

const JoinGame = () => {
  const joinGame = async () => {};

  useLocalSyncConsumer("GameCreated", (payload) => {
    console.log("GameCreated: " + JSON.stringify(payload));
  });

  return (
    <PageTemplate>
      <Tile text="Join Game" blue onClick={joinGame} />
    </PageTemplate>
  );
};

export default JoinGame;
