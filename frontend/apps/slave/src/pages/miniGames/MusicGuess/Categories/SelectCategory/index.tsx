import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { MusicGuessInteractions } from "@repo/ui/minigames/actions";
import {
  MusicGuessDefinition,
  MusicGuessState,
} from "@repo/ui/api/queries/minigames/musicGuess";
import { useGame } from "@repo/ui/contexts/GameContext";

const SelectCategory = () => {
  const {
    gameId,
    miniGameDefinition: definition,
    miniGameState: state,
  } = useGame<MusicGuessState, MusicGuessDefinition>();
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const categories =
    definition?.rounds.find((round) => round.id === state?.currentRoundId)
      ?.categories ?? [];

  const onSelectHandle = async (categoryId: string) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: MusicGuessInteractions.CategorySelection,
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
