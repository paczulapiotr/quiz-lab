import { useMemo } from "react";
import { AbcdState, AbcdDefinition } from "../../../api/queries/minigames/abcd";
import { useGame } from "../../../contexts/GameContext";

export type SelectedCategory = {
  isSelected: boolean;
  text: string;
  id: string;
  players: { id: string; name: string }[];
};

export const useShowCategories = (): SelectedCategory[] => {
  const {
    players,
    miniGameDefinition: definition,
    miniGameState: state,
  } = useGame<AbcdState, AbcdDefinition>();

  return useMemo(() => {
    const categoryDefs =
      definition?.rounds.find((round) => round.id === state?.currentRoundId)
        ?.categories ?? [];
    const categorySelections =
      state?.rounds.find((round) => round.roundId === state?.currentRoundId)
        ?.selectedCategories ?? [];

    const categories =
      categoryDefs.map((c) => ({
        isSelected: c.id === state?.currentCategoryId,
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
    definition?.rounds,
    state?.currentCategoryId,
    state?.currentRoundId,
    state?.rounds,
    players,
  ]);
};
