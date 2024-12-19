import { useParams } from "react-router";

const GameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();

  return <div>{"GameEnding: " + gameId}</div>;
};

export default GameEnding;
