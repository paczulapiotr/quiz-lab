import Component from "./Component";
import { useGetAppliedPowerPlay } from "@/api/queries/minigames/abcd/useGetAppliedPowerPlay";

type Props = {
  gameId: string;
};

const ShowAppliedPowerPlay = ({ gameId }: Props) => {
  const appliedPowerPlays = useGetAppliedPowerPlay(gameId, true);

  return (
    <Component appliedPowerPlays={appliedPowerPlays.data?.powerPlays ?? []} />
  );
};

export default ShowAppliedPowerPlay;
