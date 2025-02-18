import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";

type Props = {
  gameId?: string;
};

const ShowQuestion = ({ gameId }: Props) => {
  const { answers, question } = useBoardItems(gameId);

  return <MainBoard answers={answers} question={question} />;
};

export default ShowQuestion;
