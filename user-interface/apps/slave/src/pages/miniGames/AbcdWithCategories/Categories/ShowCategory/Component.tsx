import { Tile } from "@repo/ui/components";
import styles from "./Component.module.scss";

type Props = {
  selections: {
    isSelected: boolean;
    text: string;
    id: string;
    players: { id: string; name: string }[];
  }[];
};

const Component = ({ selections }: Props) => {
  return (
    <div className={styles.grid}>
      {selections.map((x) => (
        <Tile
          selected={x.isSelected}
          text={`${x.text} - ${x.players.map((x) => x.name).join(", ")}`}
          key={x.id}
        />
      ))}
    </div>
  );
};

export default Component;
