import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import Times from "@repo/ui/config/times";

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
      <div style={{ flex: 1 }} />
      <Timer startSeconds={Times.Abdc.QestionAnswerSeconds} />
    </>
  );
};

export default Component;
