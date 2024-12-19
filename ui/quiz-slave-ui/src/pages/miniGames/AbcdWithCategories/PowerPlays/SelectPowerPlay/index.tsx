import { useState } from "react";
import SelectPowerPlayView from "./SelectPowerPlayView";
import SelectPlayerView from "./SelectPlayerView";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";

type Props = {
  gameId: string;
};

const SelectPowerPlay = ({ gameId }: Props) => {
  const [selectedPowerPlay, setSelectedPowerPlay] = useState<string>();

  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const onPowerPlaySelect = (powerPlay: PowerPlaysEnum) => {
    setSelectedPowerPlay(PowerPlaysNames[powerPlay]);
  };

  const onPlayerSelect = async (playerId: string) => {
    await sendAsync({
      gameId,
      interactionType: "powerPlay",
      data: { powerPlay: selectedPowerPlay!, playerId: playerId },
    });
  };

  return selectedPowerPlay ? (
    <SelectPlayerView onSelect={onPlayerSelect} players={[]} />
  ) : (
    <SelectPowerPlayView
      onSelect={onPowerPlaySelect}
      players={[{ id: "test_id", name: "Test Name" }]}
    />
  );
};

export default SelectPowerPlay;
