// import { useParams } from "react-router";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import Answer from "./Round/Answer";
import ShowQuestion from "./Round/ShowQuestion";
import ShowSolution from "./Round/ShowSolution";

type Props = {
  basePath: string;
};

const LettersAndPhrases = ({ basePath }: Props) => {
  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case LettersAndPhrasesActions.QuestionShow:
            case LettersAndPhrasesActions.QuestionShown:
              return "/question_show";
            case LettersAndPhrasesActions.AnswerStart:
            case LettersAndPhrasesActions.Answered:
              return "/answer";
            case LettersAndPhrasesActions.PhraseSolvedPresentation:
            case LettersAndPhrasesActions.PhraseSolvedPresented:
              return "/phrase_solved_presentation";
            default:
              return "";
          }
        }}
        routes={{
          "/question_show": <ShowQuestion />,
          "/answer": <Answer />,
          "/phrase_solved_presentation": <ShowSolution />,
        }}
      />
    </PageTemplate>
  );
};

export default LettersAndPhrases;
