import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Tile } from "@repo/ui/components";
import { useGame } from "@repo/ui/contexts/GameContext";

const Component = () => {
  const { miniGameDefinition: definition, miniGameState: state } = useGame<
    SorterState,
    SorterDefinition
  >();
  const roundDef = definition?.rounds.find(
    (x) => x.id == state?.currentRoundId,
  );
  const left = roundDef?.leftCategory.name;
  const right = roundDef?.rightCategory.name;

  return <Tile blue text={`Posortuj, ${left} czy ${right}?`} />;
};

export default Component;
