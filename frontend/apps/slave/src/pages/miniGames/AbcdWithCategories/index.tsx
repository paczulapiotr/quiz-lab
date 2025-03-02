import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import SelectCategory from "./Categories/SelectCategory";
import ShowCategory from "./Categories/ShowCategory";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import {
  PageTemplate,
  CenteredInstruction,
} from "@repo/ui/components";
import { AbcdActions } from "@repo/ui/minigames/actions";
import { useGame } from "@repo/ui/contexts/GameContext";

const AbcdWithCategories = () => {
  const { miniGameStatus} = useGame();
  return <PageTemplate squares key={miniGameStatus}>{render(miniGameStatus)}</PageTemplate>;
};

export default AbcdWithCategories;

const render = (miniGameStatus?: string) => {
  switch (miniGameStatus) {
    case AbcdActions.PowerPlayExplainStart:
      return <CenteredInstruction
        title="Obejrzyj prezentację zagrywek na monitorze na środku sali"
        secondaryText="Zasady zagrywek..."
      />;
    case AbcdActions.PowerPlayExplainStop:
      return <SelectPowerPlay />;
    case AbcdActions.PowerPlayStart:
      return <SelectPowerPlay />;
    case AbcdActions.PowerPlayApplyStart:
      return <ShowAppliedPowerPlay />;
    case AbcdActions.PowerPlayApplyStop:
      return <ShowAppliedPowerPlay />;
    case AbcdActions.CategorySelectStart:
      return <SelectCategory />;
    case AbcdActions.CategoryShowStart:
      return <ShowCategory />;
    case AbcdActions.CategoryShowStop:
      return <ShowCategory />;
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
}