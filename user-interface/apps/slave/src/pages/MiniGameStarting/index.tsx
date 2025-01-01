import { useParams } from "react-router";

const MiniGameStarting = () => {
  const { gameId } = useParams<{ gameId: string }>();

  return <div>{"MiniGameStarting: " + gameId}</div>;
};

export default MiniGameStarting;
