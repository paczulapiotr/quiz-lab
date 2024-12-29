import { Tile } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useState } from "react";

type Props = {
  categories: { text: string; id: string }[];
  onSelect: (categoryId: string) => void;
};

const SelectCategory = ({ categories, onSelect }: Props) => {
  const [selected, setSelected] = useState<string>();

  const onSelectHandle = (categoryId: string) => {
    setSelected(categoryId);
    if (selected != null) return;
    onSelect(categoryId);
  };

  return (
    <div className={styles.grid}>
      {categories.map((c) => (
        <Tile
          text={c.text}
          onClick={() => onSelectHandle(c.id)}
          key={c.id}
          selected={selected === c.id}
        />
      ))}
    </div>
  );
};

export default SelectCategory;
