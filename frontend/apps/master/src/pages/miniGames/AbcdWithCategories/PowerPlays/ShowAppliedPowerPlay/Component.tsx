import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import styles from "./Component.module.scss";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

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
  const { gameId } = useGame();
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
      <div style={{ flex: 1 }} />
      <Timer
        startSeconds={Times.Abdc.PowerPlayShowSeconds}
        onTimeUp={onTimeUp}
      />
    </>
  );
};

export default Component;
