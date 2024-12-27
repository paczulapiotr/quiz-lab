import {
  HeaderTile,
  ScoreTile,
  TileButton,
  Timer,
} from "quiz-common-ui/components";
import styles from "./Component.module.scss";
import { useState } from "react";

type Props = {
  categories: { text: string; id: string }[];
  onSelect: (categoryId: string) => void;
  score: number;
};

const SelectCategory = ({ categories, onSelect, score }: Props) => {
  const [selected, setSelected] = useState<string>();

  const onSelectHandle = (categoryId: string) => {
    setSelected(categoryId);
    if (selected != null) return;
    onSelect(categoryId);
  };

  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title="Wybierz kategorię" />
      <div className={styles.grid}>
        {categories.map((c) => (
          <TileButton
            text={c.text}
            onClick={() => onSelectHandle(c.id)}
            key={c.id}
            selected={selected === c.id}
          />
        ))}
      </div>
      <Timer startSeconds={29} />
    </>
  );
};

export default SelectCategory;
