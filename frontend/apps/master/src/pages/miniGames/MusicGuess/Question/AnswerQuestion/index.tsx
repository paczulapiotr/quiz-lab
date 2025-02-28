import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import Component from "./Component";
import { useGame } from "@repo/ui/contexts/GameContext";

const AnswerQuestion = () => {
  const { miniGameState: state, miniGameDefinition: def } = useGame<
    MusicGuessState,
    MusicGuessDefinition
  >();

  const question = def?.rounds
    .find((r) => r.id === state?.currentRoundId)
    ?.categories.find((c) => c.id === state?.currentCategoryId)
    ?.questions.find((q) => q.id === state?.currentQuestionId);

  return (
    <Component
      answers={question?.answers ?? []}
      question={question?.text}
      questionAudio={question?.audioUrl}
    />
  );
};

export default AnswerQuestion;
