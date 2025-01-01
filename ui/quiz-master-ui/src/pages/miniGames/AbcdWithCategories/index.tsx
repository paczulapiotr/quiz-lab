import { GenericNavigator, PageTemplate } from "quiz-common-ui/components";
import { SyncReceiveData } from "node_modules/quiz-common-ui/dist/services/types";
import { useParams } from "react-router";
import ShowCategories from "./Categories/ShowCategories";
import ShowSelectedCategory from "./Categories/ShowSelectedCategory";
import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import PowerPlayExplain from "./PowerPlays/PowerPlayExplain";

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
          "/powerplay_explain": <PowerPlayExplain />,
          "/powerplay_select": <SelectPowerPlay />,
          "/powerplay_apply": <ShowAppliedPowerPlay gameId={gameId!} />,
          "/category_select": <ShowCategories gameId={gameId!} />,
          "/category_show": <ShowSelectedCategory gameId={gameId!} />,
          "/question_show": <ShowQuestion gameId={gameId!} />,
          "/question_answer": <AnswerQuestion gameId={gameId!} />,
          "/question_answer_show": <ShowQuestionAnswer gameId={gameId!} />,
        }}
      />
    </PageTemplate>
  );
};

export default AbcdWithCategories;
