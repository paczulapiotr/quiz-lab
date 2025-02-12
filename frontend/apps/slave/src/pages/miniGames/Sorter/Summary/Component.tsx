import { Tile } from "@repo/ui/components";
import styles from "./Component.module.scss";
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { useMemo } from "react";

type Props = {
  gameId?: string;
};

const Component = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<SorterState, SorterDefinition>(gameId);

  const { total, correct } = useMemo(() => {
    const roundDef = data?.definition?.rounds.find(
      (x) => x.id == data.state?.currentRoundId,
    );
    const t =
      (roundDef?.leftCategory.items.length ?? 0) +
      (roundDef?.rightCategory.items.length ?? 0);
    const c =
      data?.state?.rounds
        .find((x) => x.roundId == data?.state?.currentRoundId)
        ?.answers.find((x) => x.playerId == data.playerId)?.correctAnswers ?? 0;

    return { total: t, correct: c };
  }, [
    data?.definition?.rounds,
    data?.playerId,
    data?.state?.currentRoundId,
    data?.state?.rounds,
  ]);

  return (
    <div className={styles.summary}>
      <Tile blue text={`Wynik rundy: ${correct} / ${total}`} />
    </div>
  );
};

export default Component;
