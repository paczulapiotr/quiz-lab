import { PageTemplate, ScoreTile } from "@repo/ui/components";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import ShowQuestion from "./ShowQuestion";
import AnswerQuestion from "./AnswerQuestion";
import ShowAnswer from "./ShowAnswer";
import RoundEnd from "./RoundEnd";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useGame } from "@repo/ui/contexts/GameContext";

const FamilyFeud = () => {
  const { gameId, miniGameStatus } = useGame();
  const { data } = useGetScore(gameId);

  return (
    <PageTemplate squares>
      <ScoreTile score={data?.miniGameScore} />
      {render(miniGameStatus)}
    </PageTemplate>
  );
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
