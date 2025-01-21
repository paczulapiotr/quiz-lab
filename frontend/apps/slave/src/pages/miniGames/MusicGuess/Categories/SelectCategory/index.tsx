import { useGetCategories } from "@/api/queries/minigames/music/useGetCategories";
import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@/api/queries/useGetScore";
import { MusicGuessInteractions} from "@repo/ui/minigames/actions"
type Props = {
  gameId: string;
};

const SelectCategory = ({ gameId }: Props) => {
  const { data } = useGetCategories(gameId, true);
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const onSelectHandle = async (categoryId: string) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: MusicGuessInteractions.CategorySelection,
      value: categoryId,
    });
  };

  return (
    <Component
      categories={data?.categories ?? []}
      onSelect={onSelectHandle}
      score={score?.miniGameScore ?? 0}
    />
  );
};

export default SelectCategory;
