import { Tile } from "quiz-common-ui/components";
import styles from "./Component.module.scss";

type Props = {
  answerId: string;
  answers: {
    id: string;
    text: string;
    isCorrect: boolean;
    players: { id: string; name: string }[];
  }[];
};

const Component = ({ answers, answerId }: Props) => {
  return (
    <div className={styles.grid}>
      {answers.map((a) => (
        <Tile
          key={a.id}
          text={`${a.text} - ${a.players.map((x) => x.name).join(", ")}`}
          success={a.isCorrect}
          failure={a.isCorrect ? false : a.id === answerId}
        />
      ))}
    </div>
  );
};

export default Component;
