import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import Component from "./Component";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowSelectedCategory = () => {
  const {
    players,
    miniGameState: state,
    miniGameDefinition: definition,
  } = useGame<MusicGuessState, MusicGuessDefinition>();

  const selectedCategories = state?.rounds.find(
    (r) => r.roundId === state?.currentRoundId,
  )?.selectedCategories;
  const categories = definition?.rounds.find(
    (r) => r.id === state?.currentRoundId,
  )?.categories;
  const selections =
    categories?.map((c) => ({
      isSelected: c.id === state?.currentCategoryId,
      text: c.name,
      id: c.id,
      players:
        selectedCategories
          ?.find((x) => x.categoryId === c.id)
          ?.playerIds.map((x) => ({
            id: x,
            name: players.find((p) => p.id === x)?.name ?? "",
          })) ?? [],
    })) ?? [];

  return <Component selections={selections} />;
};

export default ShowSelectedCategory;
