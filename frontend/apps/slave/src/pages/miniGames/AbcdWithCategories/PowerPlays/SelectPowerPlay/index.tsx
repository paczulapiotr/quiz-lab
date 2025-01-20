import { useGetScore } from "@/api/queries/useGetScore";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import Component from "./Component";
import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import { useGetPowerPlays } from "@/api/queries/minigames/abcd/useGetPowerPlays";
import { AbcdInteractions} from "@repo/ui/minigames/actions"

type Props = {
  gameId: string;
};

const SelectPowerPlay = ({ gameId }: Props) => {
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const powerPlays = useGetPowerPlays(gameId, true);
  const { data: score } = useGetScore(gameId);
  const sendSelectPowerPlay = async (
    powerPlay: PowerPlaysEnum,
    deviceId: string,
  ) => {
    await sendInteraction({
      gameId: gameId!,
      interactionType: AbcdInteractions.PowerPlaySelection,
      data: {
        powerPlay: PowerPlaysNames[powerPlay],
        deviceId,
      },
    });
  };

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      onSelect={sendSelectPowerPlay}
      players={powerPlays.data?.players ?? []}
    />
  );
};

export default SelectPowerPlay;
