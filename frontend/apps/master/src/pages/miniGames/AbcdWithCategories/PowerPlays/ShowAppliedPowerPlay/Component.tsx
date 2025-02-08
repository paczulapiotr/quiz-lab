import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import styles from "./Component.module.scss";
import { useParams } from "react-router";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { AbcdActions } from "@repo/ui/minigames/actions";

type Props = {
  players: {
    playerId: string;
    playerName: string;
    powerPlays: {
      playerId: string;
      playerName: string;
      powerPlay: PowerPlaysEnum;
    }[];
  }[];
};

const Component = ({ players }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateMiniGame();

  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: AbcdActions.PowerPlayApplyStop,
    });

  return (
    <>
      <HeaderTile title="Zagrane zagrywki" />
      <div className={styles.list}>
        {players.map((player) => (
          <Tile
            text={`${player.playerName} - ${player.powerPlays.map((pp) => `${pp.playerName}|${PowerPlaysNames[pp.powerPlay]}`).join(",")}`}
            key={player.playerId}
          />
        ))}
      </div>
      <Timer
        startSeconds={Times.Abdc.PowerPlayShowSeconds}
        onTimeUp={onTimeUp}
      />
    </>
  );
};

export default Component;
