import {
  MusicGuessDefinition,
  MusicGuessState,
} from "@repo/ui/api/queries/minigames/musicGuess";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const ShowCategory = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(
    gameId,
  );

  const { data: score } = useGetScore(gameId);
  const { players } = usePlayers();

  const selectedCategories = data?.state?.rounds.find(
    (r) => r.roundId === data?.state?.currentRoundId,
  )?.selectedCategories;
  const categories = data?.definition?.rounds.find(
    (r) => r.id === data?.state?.currentRoundId,
  )?.categories;
  const selections =
    categories?.map((c) => ({
      isSelected: c.id === data?.state?.currentCategoryId,
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

  return (
    <Component score={score?.miniGameScore ?? 0} selections={selections} />
  );
};

export default ShowCategory;
