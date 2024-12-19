import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import { Tile } from "quiz-common-ui/components";
import styles from "./index.module.scss";

type Props = {
  appliedPowerPlays: {
    playerId: string;
    playerName: string;
    powerPlay: PowerPlaysEnum;
  }[];
};

const ShowAppliedPowerPlay = ({ appliedPowerPlays }: Props) => {
  return (
    <div className={styles.list}>
      {appliedPowerPlays.map((appliedPowerPlay) => (
        <Tile text={PowerPlaysNames[appliedPowerPlay.powerPlay]} />
      ))}
    </div>
  );
};

export default ShowAppliedPowerPlay;
