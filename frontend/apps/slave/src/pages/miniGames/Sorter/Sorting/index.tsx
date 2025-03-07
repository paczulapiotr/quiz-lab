import Component from "./Component";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Item } from "@repo/ui/components/ItemSorter";
import shuffle from "lodash/shuffle";
import { useMemo } from "react";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { SorterInteractions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";
import Times from "@repo/ui/config/times";

type Props = {
  started?: boolean;
};

const Sorting = ({ started }: Props) => {
  const {
    gameId,
    miniGameState: state,
    miniGameDefinition: definition,
  } = useGame<SorterState, SorterDefinition>();
  const { mutateAsync } = useSendPlayerInteraction();

  const { items, left, right, leftId, rightId } = useMemo(() => {
    const roundDef = definition?.rounds.find(
      (x) => x.id == state?.currentRoundId,
    );
    const leftItems =
      roundDef?.leftCategory.items.map<Item>((x) => ({
        id: x.id,
        name: x.name,
        left: true,
      })) || [];

    const rightItems =
      roundDef?.rightCategory.items.map<Item>((x) => ({
        id: x.id,
        name: x.name,
        right: true,
      })) || [];

    const combinedItems = [...leftItems, ...rightItems];

    return {
      items: shuffle(combinedItems),
      left: roundDef?.leftCategory.name,
      leftId: roundDef?.leftCategory.id,
      right: roundDef?.rightCategory.name,
      rightId: roundDef?.rightCategory.id,
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [state?.currentRoundId, gameId]);

  const sortItem = (item: Item, categoryId: string) => {
    mutateAsync({
      gameId: gameId!,
      value: `${item.id}_${categoryId}`,
      interactionType: SorterInteractions.SortSelection,
    });
  };

  return (
    <Component
      timerSeconds={started ? Times.Sorter.AnswerSeconds : Times.Sorter.RoundStartWaitSeconds}
      items={started ? items : []}
      leftAnswer={left ?? ""}
      rightAnswer={right ?? ""}
      onLeftAnswer={(item) => sortItem(item, leftId!)}
      onRightAnswer={(item) => sortItem(item, rightId!)}
    />
  );
};

export default Sorting;
