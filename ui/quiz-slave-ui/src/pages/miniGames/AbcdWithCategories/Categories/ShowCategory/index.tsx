import { useGetSelectedCategory } from "@/api/queries/minigames/abcd/useGetSelectedCategory";
import Component from "./Component";
import { useGetScore } from "@/api/queries/useGetScore";

type Props = {
  gameId: string;
};

const ShowCategory = ({ gameId }: Props) => {
  const { data } = useGetSelectedCategory(gameId, true);
  const { data: score } = useGetScore(gameId);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      selections={data?.categories ?? []}
    />
  );
};

export default ShowCategory;
