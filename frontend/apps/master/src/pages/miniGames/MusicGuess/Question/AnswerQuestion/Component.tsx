import { AudioPlayer, HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import Times from "@repo/ui/config/times";

type Props = {
  questionAudio?: string;
  question: string;
  answers: { id: string; text: string }[];
};

const Component = ({ answers, question, questionAudio }: Props) => {
  return (
    <>
      {questionAudio ? <AudioPlayer src={questionAudio} play /> : null}
      <HeaderTile title={question} />
      <div className={styles.grid}>
        {answers.map((x) => (
          <Tile key={x.id} text={x.text} />
        ))}
      </div>
      <Timer startSeconds={Times.Music.QestionAnswerSeconds} />
    </>
  );
};

export default Component;
