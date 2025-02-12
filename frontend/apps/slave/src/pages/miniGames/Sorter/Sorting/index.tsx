import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Item } from "@repo/ui/components/ItemSorter";
import shuffle from "lodash/shuffle";
import { useMemo } from "react";

type Props = {
  start?: boolean;
  gameId?: string;
};

const Sorting = ({ start, gameId }: Props) => {
  const { data } = useGetMiniGame<SorterState, SorterDefinition>(gameId);

  const { items, left, right } = useMemo(() => {
    const roundDef = data?.definition?.rounds.find(
      (x) => x.id == data?.state?.currentRoundId,
    );
    const leftItems =
      roundDef?.leftCategory.items.map<Item>((x) => ({
        id: x.id,
        name: x.text,
        left: true,
      })) || [];

    const rightItems =
      roundDef?.rightCategory.items.map<Item>((x) => ({
        id: x.id,
        name: x.text,
        right: true,
      })) || [];

    const combinedItems = [...leftItems, ...rightItems];

    return {
      items: shuffle(combinedItems),
      left: roundDef?.leftCategory.name,
      right: roundDef?.rightCategory.name,
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data?.state?.currentRoundId, gameId]);

  return (
    <Component
      items={start ? items : []}
      leftAnswer={left ?? ""}
      rightAnswer={right ?? ""}
    />
  );
};

export default Sorting;
