import Component from "./Component";
import { useGetAppliedPowerPlay } from "@/api/queries/minigames/abcd/useGetAppliedPowerPlay";

type Props = {
  gameId: string;
};

const ShowAppliedPowerPlay = ({ gameId }: Props) => {
  const appliedPowerPlays = useGetAppliedPowerPlay(gameId, true);
  console.log(appliedPowerPlays.data);
  return (
    <Component
      appliedPowerPlays={appliedPowerPlays.data?.players[0]?.powerPlays ?? []}
    />
  );
};

export default ShowAppliedPowerPlay;
