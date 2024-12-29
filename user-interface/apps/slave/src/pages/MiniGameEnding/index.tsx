import { useParams } from "react-router";

const MiniGameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();

  return <div>{"MiniGameEnding: " + gameId}</div>;
};

export default MiniGameEnding;
