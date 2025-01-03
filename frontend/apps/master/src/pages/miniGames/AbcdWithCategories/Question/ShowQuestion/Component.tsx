import { HeaderTile, Timer } from "@repo/ui/components";
import { useLocalSync } from "@repo/ui/hooks";
import { useParams } from "react-router";

type Props = {
  questionId: string;
  question: string;
};

const Component = ({ question }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();
  const onTimeUp = () =>
    sendSync("MiniGameUpdate", {
      gameId: gameId!,
      action: "QuestionShowStop",
      miniGameType: "AbcdWithCategories",
    });

  return (
    <>
      <HeaderTile title={question} />
      <Timer startSeconds={9} onTimeUp={onTimeUp} />
    </>
  );
};

export default Component;
