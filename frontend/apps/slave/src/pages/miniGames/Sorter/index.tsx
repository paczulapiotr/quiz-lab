import { PageTemplate } from "@repo/ui/components";
import { SorterActions } from "@repo/ui/minigames/actions";
import Sorting from "./Sorting";
import Summary from "./Summary";
import { useGame } from "@repo/ui/contexts/GameContext";

const Sorter = () => {
  const { miniGameStatus } = useGame();
  return <PageTemplate squares>{render(miniGameStatus)}</PageTemplate>;
};

export default Sorter;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case SorterActions.RoundStart:
      return <Sorting />;
    case SorterActions.RoundStarted:
      return <Sorting started />;
    case SorterActions.RoundEnd:
    case SorterActions.RoundSummary:
      return <Summary />;
    default:
      return null;
  }
};
