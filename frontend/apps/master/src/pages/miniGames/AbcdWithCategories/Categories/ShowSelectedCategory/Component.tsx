import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useParams } from "react-router";
import { useUpdateMiniGame } from "@/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";

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
  const { mutate } = useUpdateMiniGame();
  
  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: "CategoryShowStop",
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
      <Timer startSeconds={Times.Abdc.CategorShowSeconds} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
