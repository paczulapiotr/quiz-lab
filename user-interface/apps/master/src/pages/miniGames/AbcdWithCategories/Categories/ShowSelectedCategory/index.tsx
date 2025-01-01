import { useGetSelectedCategory } from "@/api/queries/minigames/abcd/useGetSelectedCategory";
import Component from "./Component";

type Props = {
  gameId: string;
};

const ShowSelectedCategory = ({ gameId }: Props) => {
  const { data } = useGetSelectedCategory(gameId, true);

  return <Component selections={data?.categories ?? []} />;
};

export default ShowSelectedCategory;
