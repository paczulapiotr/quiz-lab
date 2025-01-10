import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import styles from "./Component.module.scss";
import Times from "@repo/ui/config/times";

type Props = {
  appliedPowerPlays: {
    playerId: string;
    playerName: string;
    powerPlay: PowerPlaysEnum;
  }[];
};

const Component = ({ appliedPowerPlays }: Props) => {
  return (
    <>
      <HeaderTile title="Zagrane zagrywki" />
      <div className={styles.list}>
        {appliedPowerPlays.map((appliedPowerPlay) => (
          <Tile
            text={`${PowerPlaysNames[appliedPowerPlay.powerPlay]} - ${appliedPowerPlay.playerName}`}
            key={appliedPowerPlay.playerId}
          />
        ))}
      </div>
      <Timer startSeconds={Times.Abdc.PowerPlayShowSeconds} />
    </>
  );
};

export default Component;
