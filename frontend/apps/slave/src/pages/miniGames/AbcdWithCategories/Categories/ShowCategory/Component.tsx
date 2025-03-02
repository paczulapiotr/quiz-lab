import { ScoreTile, HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import Times from "@repo/ui/config/times";

type Props = {
  selections: {
    isSelected: boolean;
    text: string;
    id: string;
    players: { id: string; name: string }[];
  }[];
  score: number;
};

const Component = ({ selections, score }: Props) => {
  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title="Wybrana kategoria" />
      <div className={styles.grid}>
        {selections.map((x) => (
          <Tile
            selected={x.isSelected}
            text={`${x.text} - ${x.players.map((x) => x.name).join(", ")}`}
            key={x.id}
          />
        ))}
      </div>
      <div style={{ flex: 1 }} />
      <Timer startSeconds={Times.Abdc.CategoryShowSeconds} />
    </>
  );
};

export default Component;
