import { PageTemplate } from "@repo/ui/components";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import Round from "./Round";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { useCallback } from "react";
import { useGame } from "@repo/ui/contexts/GameContext";

const LettersAndPhrases = () => {
  const { gameId, miniGameStatus } = useGame();
  const { mutate } = useUpdateMiniGame();

  const onQuestionShown = useCallback(
    () =>
      mutate({
        gameId: gameId!,
        action: LettersAndPhrasesActions.QuestionShown,
      }),
    [gameId, mutate],
  );

  const onSolved = useCallback(() => {
    mutate({
      gameId: gameId!,
      action: LettersAndPhrasesActions.PhraseSolvedPresented,
    });
  }, [gameId, mutate]);

  const render = useCallback(
    (miniGameStatus?: string) => {
      switch (miniGameStatus) {
        case LettersAndPhrasesActions.QuestionShow:
          return (
            <Round
              onTimeUp={onQuestionShown}
              startSeconds={Times.Letters.ShowPhraseSeconds}
            />
          );
        case LettersAndPhrasesActions.QuestionShown:
          return <Round />;
        case LettersAndPhrasesActions.AnswerStart:
          return <Round startSeconds={Times.Letters.AnswerSeconds} />;
        case LettersAndPhrasesActions.Answered:
          return <Round />;
        case LettersAndPhrasesActions.PhraseSolvedPresentation:
          return (
            <Round
              onTimeUp={onSolved}
              startSeconds={Times.Letters.ShowResolvedPhraseSeconds}
            />
          );
        case LettersAndPhrasesActions.PhraseSolvedPresented:
          return <Round />;
        default:
          return null;
      }
    },
    [onQuestionShown, onSolved],
  );

  return <PageTemplate squares key={miniGameStatus}>{render(miniGameStatus)}</PageTemplate>;
};

export default LettersAndPhrases;
