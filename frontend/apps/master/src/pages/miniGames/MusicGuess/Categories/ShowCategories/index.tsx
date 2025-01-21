import { useGetCategories } from "@/api/queries/minigames/music/useGetCategories";
import Component from "./Component";

type Props = {
  gameId: string;
};

const ShowCategories = ({ gameId }: Props) => {
  const { data } = useGetCategories(gameId, true);

  return <Component categories={data?.categories ?? []} />;
};

export default ShowCategories;
