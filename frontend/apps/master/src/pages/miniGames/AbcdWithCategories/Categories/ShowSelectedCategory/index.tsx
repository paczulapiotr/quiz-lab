import Component from "./Component";
import { useShowCategories } from "@repo/ui/hooks/minigames/AbcdWithCategories/useShowCategories";

type Props = {
  gameId: string;
};

const ShowSelectedCategory = ({ gameId }: Props) => {
  const selections = useShowCategories(gameId);

  return <Component selections={selections} />;
};

export default ShowSelectedCategory;
