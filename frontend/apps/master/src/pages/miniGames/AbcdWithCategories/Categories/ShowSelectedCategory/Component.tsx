import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { useLocalSync } from "@repo/ui/hooks";
import styles from "./Component.module.scss";
import { useParams } from "react-router";

type Props = {
  selections: {
    isSelected: boolean;
    text: string;
    id: string;
    players: { id: string; name: string }[];
  }[];
};

const Component = ({ selections }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();
  const onTimeUp = () =>
    sendSync("MiniGameUpdate", {
      gameId: gameId!,
      action: "CategoryShowStop",
      miniGameType: "AbcdWithCategories",
    });

  return (
    <>
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
      <Timer startSeconds={9} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
