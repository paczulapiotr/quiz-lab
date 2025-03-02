import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { MusicGuessActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";
import styles from "./Component.module.scss";

type Props = {
  answers: {
    id: string;
    text?: string;
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
      action: MusicGuessActions.QuestionAnswerShowStop,
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
