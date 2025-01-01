import { useParams } from "react-router";

const RulesExplaining = () => {
  const { gameId } = useParams<{ gameId: string }>();

  return <div>{"RulesExplaining: " + gameId}</div>;
};

export default RulesExplaining;
