import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";

type Props = {
  categories: { text: string; id: string }[];
};

const Component = ({ categories }: Props) => {
  return (
    <>
      <HeaderTile title="Wybierz kategorię" />
      <div className={styles.grid}>
        {categories.map((c) => (
          <Tile text={c.text} key={c.id} />
        ))}
      </div>
      <Timer startSeconds={29} />
    </>
  );
};

export default Component;
