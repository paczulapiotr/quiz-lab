import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import {
  MusicGuessDefinition,
  MusicGuessState,
} from "@repo/ui/api/queries/minigames/musicGuess";
import Component from "./Component";
import { useGame } from "@repo/ui/contexts/GameContext";

const SelectCategory = () => {
  const {
    gameId,
    miniGameState: state,
    miniGameDefinition: definition,
  } = useGame<MusicGuessState, MusicGuessDefinition>();
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const categories =
    definition?.rounds
      .find((x) => x.id === state?.currentRoundId)
      ?.categories.map((x) => ({ text: x.name, id: x.id })) ?? [];

  const onSelectHandle = async (categoryId: string) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: AbcdInteractions.CategorySelection,
      value: categoryId,
    });
  };

  return (
    <Component
      categories={categories}
      onSelect={onSelectHandle}
      score={score?.miniGameScore ?? 0}
    />
  );
};

export default SelectCategory;
