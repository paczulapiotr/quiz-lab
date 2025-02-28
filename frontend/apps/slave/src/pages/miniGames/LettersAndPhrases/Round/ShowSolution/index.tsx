import Component from "./Component";
import { useLetters } from "@repo/ui/hooks/miniGames/LettersAndPhrases/useLetters";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowSolution = () => {
  const { gameId } = useGame();
  const { data: score } = useGetScore(gameId);
  const { phrase } = useLetters();

  return <Component score={score?.miniGameScore ?? 0} phrase={phrase} />;
};

export default ShowSolution;
