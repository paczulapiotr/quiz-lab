import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import styles from "./Component.module.scss";
import { useParams } from "react-router";
import { useUpdateMiniGame } from "@/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";

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
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateMiniGame();
  
  const onTimeUp = () =>
    mutate({
      gameId: gameId!,
      action: "QuestionAnswerShowStop",
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
      <Timer startSeconds={Times.Abdc.AnswerShowSeconds} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
