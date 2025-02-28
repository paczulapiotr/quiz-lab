import Component from "./Component";
import { useShowCategories } from "@repo/ui/hooks/minigames/AbcdWithCategories/useShowCategories";



const ShowSelectedCategory = () => {
  const selections = useShowCategories();

  return <Component selections={selections} />;
};

export default ShowSelectedCategory;
