import Component from "./Component";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowCategories = () => {
  const { miniGameDefinition: definition, miniGameState: state } = useGame<
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
