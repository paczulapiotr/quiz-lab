import ShowCategories from "./Categories/ShowCategories";
import ShowSelectedCategory from "./Categories/ShowSelectedCategory";
import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import PowerPlayExplain from "./PowerPlays/PowerPlayExplain";
import { PageTemplate } from "@repo/ui/components";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

const AbcdWithCategories = () => {
  const { miniGameStatus } = useGame();
  return <PageTemplate squares key={miniGameStatus}>{render(miniGameStatus)}</PageTemplate>;
};

export default AbcdWithCategories;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case AbcdActions.PowerPlayExplainStart:
    case AbcdActions.PowerPlayExplainStop:
      return <PowerPlayExplain />;
    case AbcdActions.PowerPlayStart:
      return <SelectPowerPlay />;
    case AbcdActions.PowerPlayApplyStart:
    case AbcdActions.PowerPlayApplyStop:
      return <ShowAppliedPowerPlay />;
    case AbcdActions.CategorySelectStart:
      return <ShowCategories />;
    case AbcdActions.CategoryShowStart:
      return <ShowSelectedCategory />;
    case AbcdActions.CategoryShowStop:
      return <ShowSelectedCategory />;
    case AbcdActions.QuestionShowStart:
    case AbcdActions.QuestionShowStop:
      return <ShowQuestion />;
    case AbcdActions.QuestionAnswerStart:
      return <AnswerQuestion />;
    case AbcdActions.QuestionAnswerShowStart:
      return <ShowQuestionAnswer />;
    case AbcdActions.QuestionAnswerShowStop:
      return <ShowQuestionAnswer />;
    default:
      return null;
  }
};
