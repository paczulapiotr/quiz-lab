import { Tile } from "quiz-common-ui/components";
import styles from "./SelectPowerPlayView.module.scss";
import { PowerPlaysEnum } from "../types";
import { useState } from "react";

type Props = {
  onSelect: (powerPlay: PowerPlaysEnum, playerId: string) => void;
  players: {
    id: string;
    name: string;
  }[];
};

const SelectPowerPlayView = ({ onSelect, players }: Props) => {
  const [powerPlay, setPowerPlay] = useState<PowerPlaysEnum>();
  const [playerId, setPlayerId] = useState<string>();

  const choosePowerPlay = (powerPlay: PowerPlaysEnum) => {
    if (powerPlay !== undefined) return;
    setPowerPlay(powerPlay);
  };

  const choosePlayer = (_playerId: string) => {
    if (playerId !== undefined) return;

    setPlayerId(playerId);
    onSelect(powerPlay!, _playerId);
  };

  return powerPlay == null ? (
    <div className={styles.grid}>
      <Tile
        text="Slime"
        onClick={() => choosePowerPlay(PowerPlaysEnum.Slime)}
        selected={powerPlay === PowerPlaysEnum.Slime}
      />
      <Tile
        text="Freeze"
        onClick={() => choosePowerPlay(PowerPlaysEnum.Freeze)}
        selected={powerPlay === PowerPlaysEnum.Freeze}
      />
      <Tile
        text="Bombs"
        onClick={() => choosePowerPlay(PowerPlaysEnum.Bombs)}
        selected={powerPlay === PowerPlaysEnum.Bombs}
      />
      <Tile
        text="Letters"
        onClick={() => choosePowerPlay(PowerPlaysEnum.Letters)}
        selected={powerPlay === PowerPlaysEnum.Letters}
      />
    </div>
  ) : (
    <div className={styles.grid}>
      {players.map((player) => (
        <Tile
          selected={playerId === player.id}
          onClick={() => choosePlayer(player.id)}
          key={player.id}
          text={player.name}
        />
      ))}
    </div>
  );
};

export default SelectPowerPlayView;
