import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import { HeaderTile, Tile, Timer } from "quiz-common-ui/components";
import styles from "./Component.module.scss";

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
      <Timer startSeconds={9} />
    </>
  );
};

export default Component;
