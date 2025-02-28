import { PageTemplate } from "@repo/ui/components";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import Answer from "./Round/Answer";
import ShowQuestion from "./Round/ShowQuestion";
import ShowSolution from "./Round/ShowSolution";
import { useGame } from "@repo/ui/contexts/GameContext";

const LettersAndPhrases = () => {
  const { miniGameStatus } = useGame();
  return <PageTemplate squares>{render(miniGameStatus)}</PageTemplate>;
};

export default LettersAndPhrases;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case LettersAndPhrasesActions.QuestionShow:
    case LettersAndPhrasesActions.QuestionShown:
      return <ShowQuestion />;
    case LettersAndPhrasesActions.AnswerStart:
    case LettersAndPhrasesActions.Answered:
      return <Answer />;
    case LettersAndPhrasesActions.PhraseSolvedPresentation:
    case LettersAndPhrasesActions.PhraseSolvedPresented:
      return <ShowSolution />;
    default:
      return null;
  }
};
