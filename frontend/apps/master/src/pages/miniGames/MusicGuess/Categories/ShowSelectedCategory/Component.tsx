import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { MusicGuessActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  selections: {
    isSelected: boolean;
    text: string;
    id: string;
    players: { id: string; name: string }[];
  }[];
};

const Component = ({ selections }: Props) => {
  const { gameId } = useGame();
  const { mutate } = useUpdateMiniGame();

  const onTimeUp = () => {
    mutate({
      gameId: gameId!,
      action: MusicGuessActions.CategoryShowStop,
    });
  };

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
      <div style={{ flex: 1 }} />
      <Timer
        startSeconds={Times.Music.CategoryShowSeconds}
        onTimeUp={onTimeUp}
      />
    </>
  );
};

export default Component;
