import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdInteractions} from "@repo/ui/minigames/actions"
import { MusicGuessDefinition, MusicGuessState } from "@repo/ui/api/queries/minigames/musicGuess";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import Component from "./Component";

type Props = {
  gameId: string;
};

const SelectCategory = ({ gameId }: Props) => {
    const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(gameId);
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const categories = data?.definition?.rounds
    .find(x => x.id === data.state?.currentRoundId)?.categories
    .map(x => ({ text: x.name, id: x.id })) ?? [];


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
