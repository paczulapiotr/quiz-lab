import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const ShowCategories = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const categories = data?.definition?.rounds.find(x => x.id === data.state?.currentRoundId)
    ?.categories.map(x => ({ text: x.name, id: x.id })) ?? [];

  return <Component categories={categories} />;
};

export default ShowCategories;
