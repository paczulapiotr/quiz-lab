import { useState } from "react";
import { ScoreTile, HeaderTile, TileButton, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";
import styles from "./Component.module.scss";

type Props = {
  question?: string;
  answers: { id: string; text?: string }[];
  onAnswer: (answerId: string) => void;
  score: number;
};

const Component = ({
  answers,
  onAnswer,
  question,
  score,
}: Props) => {
  const [selected, setSelected] = useState<string>();

  const answerHandle = (ansId: string) => {
    if (selected != null) return;
    setSelected(ansId);
    onAnswer(ansId);
  };

  return (
    <>
      <ScoreTile score={score} />
      {question ? <HeaderTile title={question} /> : null}
      <div className={styles.grid}>
        {answers.map((x) => (
          <TileButton
            key={x.id}
            text={x.text ?? ""}
            onClick={() => answerHandle(x.id)}
            selected={selected === x.id}
          />
        ))}
      </div>
      <Timer startSeconds={Times.Music.QestionAnswerSeconds} />
    </>
  );
};

export default Component;
