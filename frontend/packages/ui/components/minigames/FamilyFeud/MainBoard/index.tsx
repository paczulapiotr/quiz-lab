import { Tile } from "@repo/ui/components";
import styles from "./index.module.scss";
import classNames from "classnames";

type Props = {
  question: string;
  answers: Answer[];
  wrongAnswer?: string;
};

export type Answer = {
  id: string;
  text: string;
  points: number;
  show: boolean;
  highlight?: boolean;
};

const MainBoard = ({ answers, question, wrongAnswer }: Props) => {
  return (
    <div className={styles.board}>
      <Tile blue text={question} />
      <ol className={styles.answers}>
        {answers.map(({ id, points, show, text, highlight }) => (
          <li
            className={classNames({ [styles.highlight]: highlight })}
            key={id}
          >
            {show ? `${text} - ${points}` : "..."}
          </li>
        ))}
      </ol>
      {wrongAnswer && <Tile failure text={wrongAnswer} />}
    </div>
  );
};

export default MainBoard;
