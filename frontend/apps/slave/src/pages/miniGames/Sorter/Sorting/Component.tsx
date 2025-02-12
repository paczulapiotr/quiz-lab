import ItemSorter, { Item } from "@repo/ui/components/ItemSorter";
import styles from "./Component.module.scss";

export type Props = {
  leftAnswer: string;
  rightAnswer: string;
  items: Item[];
}

const Component = ({items,leftAnswer,rightAnswer}:Props) => {
  return (
    <div className={styles.page}>
      <ItemSorter
        children={(item, drag) => (
          <div
            className={styles.item}
            style={{
              transform: drag ? "scale(1.1)" : undefined,
            }}
          >
            <span className={styles.text}>{item.name}</span>
          </div>
        )}
        left={
          <div className={styles.leftContainer}>
            <h1 className={styles.title}>{leftAnswer}</h1>
          </div>
        }
        right={
          <div className={styles.rightContainer}>
            <h1 className={styles.title}>{rightAnswer}</h1>
          </div>
        }
        items={items}
        onAllSorted={() => console.log("ALL SORTED")}
        onAssignLeft={(i) => console.log("LEFT " + i.name)}
        onAssignRight={(i) => console.log("LEFT " + i.name)}
      />
    </div>
  );
};

export default Component;
