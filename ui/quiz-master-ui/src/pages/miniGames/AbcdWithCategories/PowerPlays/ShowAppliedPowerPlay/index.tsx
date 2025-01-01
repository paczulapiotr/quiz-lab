import { useGetAppliedPowerPlay } from "@/api/queries/minigames/abcd/useGetAppliedPowerPlay";
import Component from "./Component";

type Props = {
  gameId: string;
};

const ShowAppliedPowerPlay = ({ gameId }: Props) => {
  const appliedPowerPlays = useGetAppliedPowerPlay(gameId, true);

  return <Component players={appliedPowerPlays.data?.players ?? []} />;
};

export default ShowAppliedPowerPlay;
