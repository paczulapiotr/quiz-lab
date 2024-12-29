import { useGetCategories } from "@/api/queries/minigames/abcd/useGetCategories";
import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import Component from "./Component";

type Props = {
  gameId: string;
};

const SelectCategory = ({ gameId }: Props) => {
  const { data } = useGetCategories(gameId, true);
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();

  const onSelectHandle = async (categoryId: string) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: "CategorySelection",
      value: categoryId,
    });
  };

  return (
    <Component categories={data?.categories ?? []} onSelect={onSelectHandle} />
  );
};

export default SelectCategory;
