import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  answers: {
    id: string;
    text: string;
    isCorrect: boolean;
  }[];
  players: {
    id: string;
    name: string;
    answerId?: string;
    answerPoints: number;
    roundPoints: number;
  }[];
};

const Component = ({ answers, players }: Props) => {
  const { gameId } = useGame();
  const { mutate } = useUpdateMiniGame();

  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: AbcdActions.QuestionAnswerShowStop,
    });

  return (
    <>
      <HeaderTile title="Odpowiedzi" />
      <div className={styles.grid}>
        {answers.map((a) => (
          <Tile
            key={a.id}
            text={`${a.text} - ${players
              .filter((x) => x.answerId === a.id)
              .map((x) => `[${x.name}|${x.answerPoints}]`)
              .join(", ")}`}
            success={a.isCorrect}
            failure={!a.isCorrect}
          />
        ))}
      </div>
      <div style={{ flex: 1 }} />
      <Timer startSeconds={Times.Abdc.AnswerShowSeconds} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
