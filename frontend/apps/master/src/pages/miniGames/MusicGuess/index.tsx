import { useParams } from "react-router";
import ShowCategories from "./Categories/ShowCategories";
import ShowSelectedCategory from "./Categories/ShowSelectedCategory";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import { SyncReceiveData } from "@repo/ui/services/types";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { MusicGuessActions } from "@repo/ui/minigames/actions";
type Props = {
  basePath: string;
};

const MusicGuess = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();

  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case MusicGuessActions.CategorySelectStart:
              return "/category_select";
            case MusicGuessActions.CategoryShowStart:
              return "/category_show";
            case MusicGuessActions.CategoryShowStop:
              return "/category_show";
            case MusicGuessActions.QuestionAnswerStart:
              return "/question_answer";
            case MusicGuessActions.QuestionAnswerShowStart:
              return "/question_answer_show";
            case MusicGuessActions.QuestionAnswerShowStop:
              return "/question_answer_show";
            default:
              return "";
          }
        }}
        routes={{
          "/category_select": <ShowCategories gameId={gameId!} />,
          "/category_show": <ShowSelectedCategory gameId={gameId!} />,
          "/question_answer": <AnswerQuestion gameId={gameId!} />,
          "/question_answer_show": <ShowQuestionAnswer gameId={gameId!} />,
        }}
      />
    </PageTemplate>
  );
};

export default MusicGuess;
