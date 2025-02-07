import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import Component from "./Component";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const SelectPowerPlay = ({ gameId }: Props) => {
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { data: score } = useGetScore(gameId);
  const { players } = usePlayers();

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
      players={players}
    />
  );
};

export default SelectPowerPlay;
