import { PowerPlaysEnum, PowerPlaysNames } from "../types";
import { HeaderTile, Tile, Timer } from "quiz-common-ui/components";
import styles from "./Component.module.scss";
import { useLocalSync } from "quiz-common-ui/hooks";
import { useParams } from "react-router";

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
  const { sendSync } = useLocalSync();
  const onTimeUp = () =>
    sendSync("MiniGameUpdate", {
      gameId: gameId!,
      action: "PowerPlayApplyStop",
      miniGameType: "AbcdWithCategories",
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
      <Timer startSeconds={9} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;