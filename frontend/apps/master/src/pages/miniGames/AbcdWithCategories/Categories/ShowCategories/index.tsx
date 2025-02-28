import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";
import Component from "./Component";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowCategories = () => {
  const { miniGameState: state, miniGameDefinition: definition } = useGame<
    AbcdState,
    AbcdDefinition
  >();
  const categories =
    definition?.rounds
      .find((x) => x.id === state?.currentRoundId)
      ?.categories.map((x) => ({ text: x.name, id: x.id })) ?? [];

  return <Component categories={categories} />;
};

export default ShowCategories;
