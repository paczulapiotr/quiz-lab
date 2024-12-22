import { useState } from "react";
import { Tile } from "quiz-common-ui/components";
import styles from "./Component.module.scss";

type Props = {
  question: string;
  answers: { id: string; text: string }[];
  onAnswer: (answerId: string) => void;
};

const Component = ({ answers, onAnswer, question }: Props) => {
  const [selected, setSelected] = useState<string>();

  const answerHandle = (ansId: string) => {
    setSelected(ansId);
    if (selected != null) return;
    onAnswer(ansId);
  };

  return (
    <div>
      <Tile text={question} />
      <div className={styles.grid}>
        {answers.map((x) => (
          <Tile
            key={x.id}
            text={x.text}
            onClick={() => answerHandle(x.id)}
            selected={selected === x.id}
          />
        ))}
      </div>
    </div>
  );
};

export default Component;
