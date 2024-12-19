import { Tile } from "quiz-common-ui/components";
import { useState } from "react";
import styles from "./SelectPlayerView.module.scss";

type Props = {
  players: {
    id: string;
    name: string;
  }[];
  onSelect: (playerId: string) => void;
};

const SelectPlayerView = ({ players, onSelect }: Props) => {
  const [selected, setSelected] = useState<string>();

  const choosePlayer = (playerId: string) => {
    if (selected !== undefined) return;

    setSelected(playerId);
    onSelect(playerId);
  };

  return (
    <div className={styles.grid}>
      {players.map((player) => (
        <Tile
          selected={selected === player.id}
          onClick={() => choosePlayer(player.id)}
          key={player.id}
          text={player.name}
        />
      ))}
    </div>
  );
};

export default SelectPlayerView;
