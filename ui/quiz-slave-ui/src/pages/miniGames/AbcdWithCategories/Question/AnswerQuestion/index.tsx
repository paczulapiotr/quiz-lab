import { useSendPlayerInteraction } from "@/api/mutations/useSendPlayerInteraction";
import { Tile } from "quiz-common-ui/components";
import styles from "./index.module.scss";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();
  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: "QuestionAnswer", value });
  return (
    <div className={styles.grid}>
      <Tile text="A" onClick={() => answer("answer_01")} />
      <Tile text="B" onClick={() => answer("answer_02")} />
      <Tile text="C" onClick={() => answer("answer_03")} />
      <Tile text="D" onClick={() => answer("answer_04")} />
    </div>
  );
};

export default AnswerQuestion;
