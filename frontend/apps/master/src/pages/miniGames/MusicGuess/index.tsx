import ShowCategories from "./Categories/ShowCategories";
import ShowSelectedCategory from "./Categories/ShowSelectedCategory";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import { PageTemplate } from "@repo/ui/components";
import { MusicGuessActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

const MusicGuess = () => {
  const { miniGameStatus } = useGame();
  return <PageTemplate squares key={miniGameStatus}>{render(miniGameStatus)}</PageTemplate>;
};

export default MusicGuess;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case MusicGuessActions.CategorySelectStart:
      return <ShowCategories />;
    case MusicGuessActions.CategoryShowStart:
    case MusicGuessActions.CategoryShowStop:
      return <ShowSelectedCategory />;
    case MusicGuessActions.QuestionAnswerStart:
      return <AnswerQuestion />;
    case MusicGuessActions.QuestionAnswerShowStart:
    case MusicGuessActions.QuestionAnswerShowStop:
      return <ShowQuestionAnswer />;
    default:
      return null;
  }
};
