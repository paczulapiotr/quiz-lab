import { HeaderTile, ScoreTile, Tile, Timer } from "quiz-common-ui/components";
import styles from "./Component.module.scss";

type Props = {
  score: number;
  answerScore: number;
  answerId?: string;
  answers: {
    id: string;
    text: string;
    isCorrect: boolean;
  }[];
};

const Component = ({ answers, answerId, answerScore, score }: Props) => {
  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title="Odpowiedzi" />
      <div className={styles.grid}>
        {answers.map((a) => (
          <Tile
            key={a.id}
            text={`${a.text}${a.id === answerId ? ` - +${answerScore}` : ""}`}
            success={a.isCorrect}
            failure={a.isCorrect ? false : a.id === answerId}
          />
        ))}
      </div>
      <Timer startSeconds={9} />
    </>
  );
};

export default Component;
