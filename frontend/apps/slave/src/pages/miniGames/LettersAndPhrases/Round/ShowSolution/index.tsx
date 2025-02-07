import Component from "./Component";
import { useLetters } from "../useLetters";
import { useParams } from "react-router";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";

const ShowSolution = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data: score } = useGetScore(gameId);
  const { phrase } = useLetters(gameId);

  return <Component score={score?.miniGameScore ?? 0} phrase={phrase} />;
};

export default ShowSolution;
