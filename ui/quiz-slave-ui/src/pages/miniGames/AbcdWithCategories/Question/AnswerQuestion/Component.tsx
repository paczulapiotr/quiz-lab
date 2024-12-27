import { useMemo, useState } from "react";
import {
  HeaderTile,
  ScoreTile,
  TileButton,
  Timer,
} from "quiz-common-ui/components";
import styles from "./Component.module.scss";
import { PowerPlaysEnum } from "../../PowerPlays/types";
import { applyLetterPowerPlay } from "./PowerPlays/helpers";
import Bombs from "./PowerPlays/Bombs";

type Props = {
  question: string;
  answers: { id: string; text: string }[];
  onAnswer: (answerId: string) => void;
  score: number;
  powerPlays: PowerPlaysEnum[];
};

const Component = ({
  answers,
  onAnswer,
  question,
  score,
  powerPlays,
}: Props) => {
  const [selected, setSelected] = useState<string>();

  const answerHandle = (ansId: string) => {
    if (selected != null) return;
    setSelected(ansId);
    onAnswer(ansId);
  };

  const answerTextDict = useMemo(() => {
    const dict: Record<string, string> = {};
    answers.forEach((x) => {
      dict[x.id] = applyLetterPowerPlay(x.text, powerPlays);
    });
    return dict;
  }, [answers, powerPlays]);

  const freezeStacks = powerPlays.filter(
    (x) => x === PowerPlaysEnum.Freeze,
  ).length;

  return (
    <>
      <ScoreTile score={score} />
      <HeaderTile title={question} />
      <div className={styles.grid}>
        {answers.map((x) => (
          <TileButton
            key={x.id}
            freezeStacks={freezeStacks}
            text={answerTextDict[x.id]}
            onClick={() => answerHandle(x.id)}
            selected={selected === x.id}
          />
        ))}
      </div>
      <Timer startSeconds={29} />
      <Bombs powerPlays={powerPlays} />
    </>
  );
};

export default Component;
