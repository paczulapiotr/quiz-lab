import Component from "./Component";
import { useLetters } from "@repo/ui/hooks/miniGames/LettersAndPhrases/useLetters";
import { useParams } from "react-router";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";

const ShowQuestion = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data: score } = useGetScore(gameId);
  const { phrase } = useLetters();

  return <Component score={score?.miniGameScore ?? 0} phrase={phrase} />;
};

export default ShowQuestion;
