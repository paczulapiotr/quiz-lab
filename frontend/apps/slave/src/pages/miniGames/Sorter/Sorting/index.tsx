import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Item } from "@repo/ui/components/ItemSorter";
import shuffle from "lodash/shuffle";
import { useMemo } from "react";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { SorterInteractions } from "@repo/ui/minigames/actions";

type Props = {
  started?: boolean;
  gameId?: string;
};

const Sorting = ({ started, gameId }: Props) => {
  const { data } = useGetMiniGame<SorterState, SorterDefinition>(gameId);
  const { mutateAsync } = useSendPlayerInteraction();

  const { items, left, right, leftId, rightId } = useMemo(() => {
    const roundDef = data?.definition?.rounds.find(
      (x) => x.id == data?.state?.currentRoundId,
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
  }, [data?.state?.currentRoundId, gameId]);

  const sortItem = (item: Item, categoryId: string) => {
    mutateAsync({
      gameId: gameId!,
      value: `${item.id}_${categoryId}`,
      interactionType: SorterInteractions.SortSelection,
    });
  };

  return (
    <Component
      items={started ? items : []}
      leftAnswer={left ?? ""}
      rightAnswer={right ?? ""}
      onLeftAnswer={(item) => sortItem(item, leftId!)}
      onRightAnswer={(item) => sortItem(item, rightId!)}
    />
  );
};

export default Sorting;
