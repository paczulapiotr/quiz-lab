import { useState } from "react";
import { TextInput, Timer } from "@repo/ui/components";
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
  
  return (
    <div className={styles.container}>
      <MainBoard answers={answers} question={question} />
      {youAreGuessing ? (
        <>
          <TextInput
            placeholder="Podaj odpowiedź"
            buttonText="Wyślij"
            onClick={onAnswer}
            value={answer}
            onChange={setAnswer}
            small
          />

          <Keyboard value={answer} onChange={setAnswer} />
        </>
      ) : (
        <div style={{ flex: 1 }} />
      )}
      <Timer startSeconds={Times.FamilyFeud.AnswerSeconds} />
    </div>
  );
};

export default Component;
