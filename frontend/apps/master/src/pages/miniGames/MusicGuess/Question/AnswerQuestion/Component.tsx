import { AudioPlayer, HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import Times from "@repo/ui/config/times";
import { fileUrl } from "@repo/ui/api/files";

type Props = {
  questionAudio?: string;
  question?: string;
  answers: { id: string; text?: string }[];
};

const Component = ({ answers, question, questionAudio }: Props) => {
  const src = questionAudio ? fileUrl(questionAudio) : null;

  return (
    <>
      {src ? <AudioPlayer src={src} play /> : null}
      {question ? <HeaderTile title={question} /> : null}
      <div className={styles.grid}>
        {answers.map((x) => (
          <Tile key={x.id} text={x.text ?? ""} />
        ))}
      </div>
      <div style={{ flex: 1 }} />
      <Timer startSeconds={Times.Music.QestionAnswerSeconds} />
    </>
  );
};

export default Component;
