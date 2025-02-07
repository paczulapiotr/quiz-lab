import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { MusicGuessInteractions } from "@repo/ui/minigames/actions";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  MusicGuessDefinition,
  MusicGuessState,
} from "@repo/ui/api/queries/minigames/musicGuess";

type Props = {
  gameId: string;
};

const SelectCategory = ({ gameId }: Props) => {
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(
    gameId,
  );

  const categories =
    data?.definition?.rounds.find(
      (round) => round.id === data?.state?.currentRoundId,
    )?.categories ?? [];

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
