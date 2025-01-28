import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const ShowCategory = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { data: score } = useGetScore(gameId);
  const { players } = usePlayers();

  const categoryDefs = data?.definition?.rounds.find((round) => round.id === data?.state?.currentRoundId)?.categories ?? []
  const categorySelections = data?.state?.rounds.find((round) => round.roundId === data?.state?.currentRoundId)?.selectedCategories ?? []

  const categories = categoryDefs.map(c => ({
    isSelected: c.id === data?.state?.currentCategoryId,
    text: c.name,
    id: c.id,
    players: categorySelections.find(sc => sc.categoryId === c.id)?.playerIds.map(id => players.find(p => p.id === id)!) ?? []
  })) ?? [];

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      selections={categories}
    />
  );
};

export default ShowCategory;
