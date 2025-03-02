import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import Component from "./Component";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";



const SelectPowerPlay = () => {
  const { mutateAsync: sendInteraction } = useSendPlayerInteraction();
  const { players, you, gameId } = useGame();
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
      players={players.filter((p) => p.id !== you?.id)}
    />
  );
};

export default SelectPowerPlay;
