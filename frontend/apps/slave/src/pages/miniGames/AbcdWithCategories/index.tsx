import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import SelectCategory from "./Categories/SelectCategory";
import ShowCategory from "./Categories/ShowCategory";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import { useParams } from "react-router";
import {
  PageTemplate,
  GenericNavigator,
  CenteredInstruction,
} from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { AbcdActions } from "@repo/ui/minigames/actions";

type Props = {
  basePath: string;
};

const AbcdWithCategories = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();

  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case AbcdActions.PowerPlayExplainStart:
              return "/powerplay_explain";
            case AbcdActions.PowerPlayExplainStop:
              return "/powerplay_explain";
            case AbcdActions.PowerPlayStart:
              return "/powerplay_select";
            case AbcdActions.PowerPlayApplyStart:
              return "/powerplay_apply";
            case AbcdActions.PowerPlayApplyStop:
              return "/powerplay_apply";
            case AbcdActions.CategorySelectStart:
              return "/category_select";
            case AbcdActions.CategoryShowStart:
              return "/category_show";
            case AbcdActions.CategoryShowStop:
              return "/category_show";
            case AbcdActions.QuestionShowStart:
            case AbcdActions.QuestionShowStop:
              return "/question_show";
            case AbcdActions.QuestionAnswerStart:
              return "/question_answer";
            case AbcdActions.QuestionAnswerShowStart:
              return "/question_answer_show";
            case AbcdActions.QuestionAnswerShowStop:
              return "/question_answer_show";
            default:
              return "";
          }
        }}
        routes={{
          "/powerplay_explain": (
            <CenteredInstruction
              title="Obejrzyj prezentację zagrywek na monitorze na środku sali"
              secondaryText="Zasady zagrywek..."
            />
          ),
          "/powerplay_select": <SelectPowerPlay gameId={gameId!} />,
          "/powerplay_apply": <ShowAppliedPowerPlay gameId={gameId!} />,
          "/category_select": <SelectCategory gameId={gameId!} />,
          "/category_show": <ShowCategory gameId={gameId!} />,
          "/question_show": <ShowQuestion gameId={gameId!} />,
          "/question_answer": <AnswerQuestion gameId={gameId!} />,
          "/question_answer_show": <ShowQuestionAnswer gameId={gameId!} />,
        }}
      />
    </PageTemplate>
  );
};

export default AbcdWithCategories;
