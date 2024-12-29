import { useParams } from "react-router";

const GameStarting = () => {
  const { gameId } = useParams<{ gameId: string }>();

  return <div>{"GameStarting: " + gameId}</div>;
};

export default GameStarting;
