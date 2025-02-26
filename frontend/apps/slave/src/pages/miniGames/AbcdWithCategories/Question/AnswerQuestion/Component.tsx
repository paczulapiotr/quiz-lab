import { useMemo, useState } from "react";

import styles from "./Component.module.scss";
import { PowerPlaysEnum } from "../../PowerPlays/types";
import { applyLetterPowerPlay } from "./PowerPlays/helpers";
import Bombs from "./PowerPlays/Bombs";
import { ScoreTile, HeaderTile, TileButton, Timer } from "@repo/ui/components";
import Times from "@repo/ui/config/times";

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

  const slimeStacks = powerPlays.filter(
    (x) => x === PowerPlaysEnum.Slime,
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
            slimeStacks={slimeStacks}
            text={answerTextDict[x.id]}
            onClick={() => answerHandle(x.id)}
            selected={selected === x.id}
          />
        ))}
      </div>
      <div style={{ flex: 1 }} />
      <Timer startSeconds={Times.Abdc.QestionAnswerSeconds} />
      <Bombs powerPlays={powerPlays} />
    </>
  );
};

export default Component;
