import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useShowCategories} from "@repo/ui/hooks/minigames/AbcdWithCategories/useShowCategories";

type Props = {
  gameId: string;
};

const ShowCategory = ({ gameId }: Props) => {
  const { data: score } = useGetScore(gameId);
  const selections = useShowCategories(gameId);
  
  return (
    <Component score={score?.miniGameScore ?? 0} selections={selections} />
  );
};

export default ShowCategory;
