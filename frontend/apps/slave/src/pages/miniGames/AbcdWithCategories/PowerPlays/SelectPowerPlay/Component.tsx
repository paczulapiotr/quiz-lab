
import styles from "./Component.module.scss";
import { PowerPlaysEnum } from "../types";
import { useState } from "react";
import { ScoreTile, HeaderTile, TileButton, CenteredInstruction, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";

type Props = {
  score: number;
  onSelect: (powerPlay: PowerPlaysEnum, deviceId: string) => void;
  players: {
    id: string;
    name: string;
  }[];
};

const Component = ({ onSelect, players, score }: Props) => {
  const [powerPlay, setPowerPlay] = useState<PowerPlaysEnum>();
  const [deviceId, setDeviceId] = useState<string>();

  const choosePowerPlay = (_powerPlay: PowerPlaysEnum) => {
    if (powerPlay !== undefined) return;
    setPowerPlay(_powerPlay);
  };

  const choosePlayer = (_deviceId: string) => {
    if (deviceId !== undefined) return;
    setDeviceId(_deviceId);
    onSelect(powerPlay!, _deviceId);
  };
  return (
    <>
      <ScoreTile score={score} />
      {powerPlay == null ? (
        <>
          <HeaderTile title="Wybierz zagrywkę" />
          <div className={styles.grid}>
            <TileButton
              text="Slime"
              onClick={() => choosePowerPlay(PowerPlaysEnum.Slime)}
              selected={powerPlay === PowerPlaysEnum.Slime}
            />
            <TileButton
              text="Freeze"
              onClick={() => choosePowerPlay(PowerPlaysEnum.Freeze)}
              selected={powerPlay === PowerPlaysEnum.Freeze}
            />
            <TileButton
              text="Bombs"
              onClick={() => choosePowerPlay(PowerPlaysEnum.Bombs)}
              selected={powerPlay === PowerPlaysEnum.Bombs}
            />
            <TileButton
              text="Letters"
              onClick={() => choosePowerPlay(PowerPlaysEnum.Letters)}
              selected={powerPlay === PowerPlaysEnum.Letters}
            />
          </div>
        </>
      ) : deviceId == null ? (
        <>
          <HeaderTile title="Wybierz gracza" />
          <div className={styles.grid}>
            {players.map((player) => (
              <TileButton
                selected={deviceId === player.id}
                onClick={() => choosePlayer(player.id)}
                key={player.id}
                text={player.name}
              />
            ))}
          </div>
        </>
      ) : (
        <CenteredInstruction title="Poczekaj na resztę" secondaryText="" />
      )}
      <Timer startSeconds={Times.Abdc.PowerPlaySelectionSeconds} />
    </>
  );
};

export default Component;
