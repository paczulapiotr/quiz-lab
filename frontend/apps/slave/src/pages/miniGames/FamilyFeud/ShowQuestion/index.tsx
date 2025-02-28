import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";

const ShowQuestion = () => {
  const { answers, question } = useBoardItems();

  return <MainBoard answers={answers} question={question} />;
};

export default ShowQuestion;
