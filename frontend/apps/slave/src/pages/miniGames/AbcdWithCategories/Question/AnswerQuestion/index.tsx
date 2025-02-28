import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const AnswerQuestion = () => {
  const { you, gameId, miniGameState: state, miniGameDefinition: definition} = useGame<AbcdState, AbcdDefinition>();
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const answer = (value: string) =>
    sendAsync({
      gameId: gameId!,
      interactionType: AbcdInteractions.QuestionAnswer,
      value,
    });

  const roundState = state?.rounds.find(
    (round) => round.roundId === state?.currentRoundId);
  
  const roundDef = definition?.rounds.find(
    (round) => round.id === state?.currentRoundId);
  
  const question = roundDef?.categories
    .find((category) => category.id === state?.currentCategoryId)
    ?.questions.find(
      (question) => question.id === state?.currentQuestionId,
  );
  
  const powerPlays = (roundState?.powerPlays[you?.id ?? ""] ?? []).map(x => x.powerPlay);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question?.answers ?? []}
      question={question?.text ?? ""}
      onAnswer={answer}
      powerPlays={powerPlays}
    />
  );
};

export default AnswerQuestion;
