import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { MusicGuessInteractions } from "@repo/ui/minigames/actions";
import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import { useGame } from "@repo/ui/contexts/GameContext";

const AnswerQuestion = () => {
  const {
    miniGameState: state,
    miniGameDefinition: definition,
    gameId,
  } = useGame<MusicGuessState, MusicGuessDefinition>();
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const question = definition?.rounds
    .find((r) => r.id === state?.currentRoundId)
    ?.categories.find((c) => c.id === state?.currentCategoryId)
    ?.questions.find((q) => q.id === state?.currentQuestionId);

  const answer = (value: string) =>
    sendAsync({
      gameId: gameId!,
      interactionType: MusicGuessInteractions.QuestionAnswer,
      value,
    });

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question?.answers ?? []}
      question={question?.text}
      onAnswer={answer}
    />
  );
};

export default AnswerQuestion;
