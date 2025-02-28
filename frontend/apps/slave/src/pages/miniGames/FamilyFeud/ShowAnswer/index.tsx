import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";

const ShowAnswer = () => {
  const { answers, question, lastWrongAnswer } = useBoardItems(true);

  return (
    <MainBoard
      answers={answers}
      question={question}
      wrongAnswer={lastWrongAnswer}
    />
  );
};

export default ShowAnswer;
