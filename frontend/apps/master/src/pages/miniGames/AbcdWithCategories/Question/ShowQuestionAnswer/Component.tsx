import { HeaderTile, Tile, Timer } from "@repo/ui/components";
import { useLocalSync } from "@repo/ui/hooks";
import styles from "./Component.module.scss";
import { useParams } from "react-router";

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
  const { sendSync } = useLocalSync();
  const onTimeUp = () =>
    sendSync("MiniGameUpdate", {
      gameId: gameId!,
      action: "QuestionAnswerShowStop",
      miniGameType: "AbcdWithCategories",
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
      <Timer startSeconds={9} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
