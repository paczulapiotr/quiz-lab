import { useState } from "react";
import { TileButton, Timer } from "@repo/ui/components";
import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import styles from "./Component.module.scss";
import { Keyboard } from "@/components/Keyboard";
import Times from "@repo/ui/config/times";

type Props = {
  onAnswer: (answer: string) => void;
};

const Component = ({ onAnswer }: Props) => {
  const [answer, setAnswer] = useState("");
  const { answers, question, youAreGuessing } = useBoardItems();
  const sendAnswer = () => onAnswer(answer);

  return (
    <div className={styles.container}>
      <MainBoard answers={answers} question={question} />
      {youAreGuessing ? (
        <>
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
        </>
      ) : null}
      <Timer startSeconds={Times.FamilyFeud.AnswerSeconds} />
    </div>
  );
};

export default Component;
