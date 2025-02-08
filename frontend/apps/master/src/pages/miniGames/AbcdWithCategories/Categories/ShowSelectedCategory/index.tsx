import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const ShowSelectedCategory = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { players } = usePlayers();
  const selectedCategories = data?.state?.rounds.find(
    (round) => round.roundId === data?.state?.currentRoundId,
  )?.selectedCategories;
  const categoryDefs = data?.definition?.rounds.find(
    (round) => round.id === data?.state?.currentRoundId,
  )?.categories;

  const categories =
    categoryDefs?.map((c) => ({
      isSelected: c.id === data?.state?.currentCategoryId,
      text: c.name,
      id: c.id,
      players:
        selectedCategories
          ?.find((sc) => sc.categoryId === c.id)
          ?.playerIds.map((pid) => players.find((p) => p.id === pid)!) ?? [],
    })) ?? [];

  return <Component selections={categories} />;
};

export default ShowSelectedCategory;
