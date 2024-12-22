import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import Component from "./Component";
import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import { useGetPowerPlays } from "@/api/queries/minigames/abcd/useGetPowerPlays";

type Props = {
  gameId: string;
};

const SelectPowerPlay = ({ gameId }: Props) => {
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const powerPlays = useGetPowerPlays(gameId, true);

  const sendSelectPowerPlay = async (
    powerPlay: PowerPlaysEnum,
    deviceId: string,
  ) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: "PowerPlaySelection",
      data: {
        powerPlay: PowerPlaysNames[powerPlay],
        deviceId,
      },
    });
  };

  return (
    <Component
      onSelect={sendSelectPowerPlay}
      players={powerPlays.data?.players ?? []}
    />
  );
};

export default SelectPowerPlay;
