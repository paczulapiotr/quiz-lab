import { useJoinGameMutation } from "@/api/mutations/useJoinGameMutation";
import { useGetCreatedGame } from "@/api/queries/useGetCreatedGame";
import { PageTemplate } from "@/components/PageTemplate";
import { Tile } from "@/components/Tile";
import { useState } from "react";
import { useParams } from "react-router";

const JoinGame = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data, isLoading } = useGetCreatedGame();
  const [playerName, setPlayerName] = useState("");
  const { mutateAsync } = useJoinGameMutation();
  console.log(data);
  const joinGame = async () => {
    console.log("Joining game: " + gameId);
    mutateAsync({ playerName });
  };

  return (
    <PageTemplate>
      <input
        type="text"
        placeholder="Your name"
        value={playerName}
        onChange={(e) => setPlayerName(e.target.value)}
      />
      <Tile text="Join Game" blue onClick={joinGame} />
      <ol>
        {isLoading ? (
          <li>{"Loading..."}</li>
        ) : (
          data?.playerNames.map((player, index) => (
            <li key={`${player}_${index}`}>{player}</li>
          ))
        )}
      </ol>
    </PageTemplate>
  );
};

export default JoinGame;
