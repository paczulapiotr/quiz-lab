import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useGame } from "@repo/ui/contexts/GameContext";
import { useShowCategories } from "@repo/ui/hooks/minigames/AbcdWithCategories/useShowCategories";

const ShowCategory = () => {
  const { gameId } = useGame();
  const { data: score } = useGetScore(gameId);
  const selections = useShowCategories();

  return (
    <Component score={score?.miniGameScore ?? 0} selections={selections} />
  );
};

export default ShowCategory;
