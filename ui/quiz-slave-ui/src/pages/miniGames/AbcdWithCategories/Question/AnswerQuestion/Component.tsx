import { useState } from "react";
import {
  HeaderTile,
  ScoreTile,
  TileButton,
  Timer,
} from "quiz-common-ui/components";
import styles from "./Component.module.scss";

type Props = {
  question: string;
  answers: { id: string; text: string }[];
  onAnswer: (answerId: string) => void;
  score: number;
};

const Component = ({ answers, onAnswer, question, score }: Props) => {
  const [selected, setSelected] = useState<string>();

  const answerHandle = (ansId: string) => {
    setSelected(ansId);
    if (selected != null) return;
    onAnswer(ansId);
  };

  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title={question} />
      <div className={styles.grid}>
        {answers.map((x) => (
          <TileButton
            key={x.id}
            text={x.text}
            onClick={() => answerHandle(x.id)}
            selected={selected === x.id}
          />
        ))}
      </div>
      <Timer startSeconds={29} />
    </>
  );
};

export default Component;
