
import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import SelectCategory from "./Categories/SelectCategory";
import ShowCategory from "./Categories/ShowCategory";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import { useParams } from "react-router";
import { PageTemplate, GenericNavigator, CenteredInstruction } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";

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
            case "PowerPlayExplainStart":
              return "/powerplay_explain";
            case "PowerPlayExplainStop":
              return "/powerplay_explain";
            case "PowerPlayStart":
              return "/powerplay_select";
            case "PowerPlayApplyStart":
              return "/powerplay_apply";
            case "PowerPlayApplyStop":
              return "/powerplay_apply";
            case "CategorySelectStart":
              return "/category_select";
            case "CategoryShowStart":
              return "/category_show";
            case "CategoryShowStop":
              return "/category_show";
            case "QuestionShowStart":
            case "QuestionShowStop":
              return "/question_show";
            case "QuestionAnswerStart":
              return "/question_answer";
            case "QuestionAnswerShowStart":
              return "/question_answer_show";
            case "QuestionAnswerShowStop":
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
