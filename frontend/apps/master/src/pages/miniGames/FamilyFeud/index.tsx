import { PageTemplate } from "@repo/ui/components";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import ShowQuestion from "./ShowQuestion";
import AnswerQuestion from "./AnswerQuestion";
import ShowAnswer from "./ShowAnswer";
import RoundEnd from "./RoundEnd";
import { useGame } from "@repo/ui/contexts/GameContext";

const FamilyFeud = () => {
  const { miniGameStatus } = useGame();
  return <PageTemplate squares>{render(miniGameStatus)}</PageTemplate>;
};

export default FamilyFeud;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case FamilyFeudActions.QuestionShow:
    case FamilyFeudActions.QuestionShown:
      return <ShowQuestion />;
    case FamilyFeudActions.AnswerStart:
    case FamilyFeudActions.Answered:
      return <AnswerQuestion />;
    case FamilyFeudActions.AnswerShow:
    case FamilyFeudActions.AnswerShown:
      return <ShowAnswer />;
    case FamilyFeudActions.RoundEnd:
    case FamilyFeudActions.RoundEnded:
      return <RoundEnd />;
    default:
      return null;
  }
};
