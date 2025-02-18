import { useState } from "react";
import { TileButton, Timer } from "@repo/ui/components";
import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import styles from "./Component.module.scss";
import { Keyboard } from "@/components/Keyboard";
import Times from "@repo/ui/config/times";

type Props = {
  gameId?: string;
  onAnswer: (answer: string) => void;
};

const Component = ({ gameId, onAnswer }: Props) => {
  const [answer, setAnswer] = useState("");
  const { answers, question } = useBoardItems(gameId);
  const sendAnswer = () => onAnswer(answer);

  return (
    <div className={styles.container}>
      <MainBoard answers={answers} question={question} />
      <div className={styles.input}>
        <input
          maxLength={40}
          className={styles.playerName}
          type="text"
          placeholder="Podaj odpowiedź"
          value={answer}
          onChange={(e) => setAnswer(e.target.value)}
        />
        <TileButton
          text={`Wyślij`}
          blue
          onClick={sendAnswer}
          className={styles.button}
        />
      </div>
      <Keyboard value={answer} onChange={(v) => setAnswer(v)} />
      <Timer startSeconds={Times.FamilyFeud.AnswerSeconds} />
    </div>
  );
};

export default Component;
