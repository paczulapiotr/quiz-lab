// import { useParams } from "react-router";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import Round from "./Round";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import { useParams } from "react-router";

type Props = {
  basePath: string;
};

const LettersAndPhrases = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { mutate } = useUpdateMiniGame();

  const onQuestionShown = () =>
    mutate({
      gameId: gameId!,
      action: LettersAndPhrasesActions.QuestionShown,
    });

  const onSolved = () => {
    mutate({
      gameId: gameId!,
      action: LettersAndPhrasesActions.PhraseSolvedPresented,
    });
  };

  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case LettersAndPhrasesActions.QuestionShow:
              return "/question_show";
            case LettersAndPhrasesActions.QuestionShown:
              return "/question_shown";
            case LettersAndPhrasesActions.AnswerStart:
              return "/answer_start";
            case LettersAndPhrasesActions.Answered:
              return "/answered";
            case LettersAndPhrasesActions.PhraseSolvedPresentation:
              return "/phrase_solved_presentation";
            case LettersAndPhrasesActions.PhraseSolvedPresented:
              return "/phrase_solved_presented";
            default:
              return "";
          }
        }}
        routes={{
          "/question_show": (
            <Round onTimeUp={onQuestionShown} startSeconds={5} />
          ),
          "/question_shown": <Round />,
          "/answer_start": <Round />,
          "/answered": <Round />,
          "/phrase_solved_presentation": (
            <Round onTimeUp={onSolved} startSeconds={5} />
          ),
          "/phrase_solved_presented": <Round />,
        }}
      />
    </PageTemplate>
  );
};

export default LettersAndPhrases;
