import SelectCategory from "./Categories/SelectCategory";
import ShowCategory from "./Categories/ShowCategory";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
import { useParams } from "react-router";
import {
  PageTemplate,
  GenericNavigator,
  CenteredInstruction,
} from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
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
          "/powerplay_explain": (
            <CenteredInstruction
              title="Obejrzyj prezentację zagrywek na monitorze na środku sali"
              secondaryText="Zasady zagrywek..."
            />
          ),
          "/category_select": <SelectCategory gameId={gameId!} />,
          "/category_show": <ShowCategory gameId={gameId!} />,
          "/question_answer": <AnswerQuestion gameId={gameId!} />,
          "/question_answer_show": <ShowQuestionAnswer gameId={gameId!} />,
        }}
      />
    </PageTemplate>
  );
};

export default MusicGuess;
