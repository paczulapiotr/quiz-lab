import { useMemo } from "react";
import { AbcdState, AbcdDefinition } from "../../../api/queries/minigames/abcd";
import { useGetMiniGame } from "../../../api/queries/useGetMiniGame";
import { usePlayers } from "../../../contexts/PlayersContext"

export type SelectedCategory = {
    isSelected: boolean;
    text: string;
    id: string;
    players: { id: string; name: string }[];
};

export const useShowCategories = (gameId?: string): SelectedCategory[] => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { players } = usePlayers();

  return useMemo(() => {
    const categoryDefs =
      data?.definition?.rounds.find(
        (round) => round.id === data?.state?.currentRoundId
      )?.categories ?? [];
    const categorySelections =
      data?.state?.rounds.find(
        (round) => round.roundId === data?.state?.currentRoundId
      )?.selectedCategories ?? [];

    const categories =
      categoryDefs.map((c) => ({
        isSelected: c.id === data?.state?.currentCategoryId,
        text: c.name,
        id: c.id,
        players:
          categorySelections
            .find((sc) => sc.categoryId === c.id)
            ?.playerIds.map((id) => players.find((p) => p.id === id)!)
            ?.filter((x) => x != null) ?? [],
      })) ?? [];

    return categories;
  }, [
    data?.definition?.rounds,
    data?.state?.currentCategoryId,
    data?.state?.currentRoundId,
    data?.state?.rounds,
    players,
  ]);
};
