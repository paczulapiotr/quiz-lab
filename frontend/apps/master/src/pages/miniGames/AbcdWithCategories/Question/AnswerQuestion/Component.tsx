import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";

type Props = {
  question: string;
  answers: { id: string; text: string }[];
};

const Component = ({ answers, question }: Props) => {
  return (
    <>
      <HeaderTile title={question} />
      <div className={styles.grid}>
        {answers.map((x) => (
          <Tile key={x.id} text={x.text} />
        ))}
      </div>
      <Timer startSeconds={29} />
    </>
  );
};

export default Component;
