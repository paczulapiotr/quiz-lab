import { ScoreTile, HeaderTile, TileButton, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useState } from "react";
import Times from "@repo/ui/config/times";

type Props = {
  categories: { name: string; id: string }[];
  onSelect: (categoryId: string) => void;
  score: number;
};

const SelectCategory = ({ categories, onSelect, score }: Props) => {
  const [selected, setSelected] = useState<string>();

  const onSelectHandle = (categoryId: string) => {
    if (selected != null) return;
    setSelected(categoryId);
    onSelect(categoryId);
  };

  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title="Wybierz kategorię" />
      <div className={styles.grid}>
        {categories.map((c) => (
          <TileButton
            text={c.name}
            onClick={() => onSelectHandle(c.id)}
            key={c.id}
            selected={selected === c.id}
          />
        ))}
      </div>
      <Timer startSeconds={Times.Music.CategorySelectionSeconds} />
    </>
  );
};

export default SelectCategory;
