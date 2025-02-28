import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";


const ShowQuestion = () => {
  const { gameId, miniGameState: state, miniGameDefinition: definition} = useGame<AbcdState, AbcdDefinition>();
  const { data: score } = useGetScore(gameId!);

  const question = definition?.rounds
    .find((round) => round.id === state?.currentRoundId)
    ?.categories.find(
      (category) => category.id === state?.currentCategoryId,
    )
    ?.questions.find(
      (question) => question.id === state?.currentQuestionId,
    );

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      question={question?.text ?? ""}
      questionId={question?.id ?? ""}
    />
  );
};

export default ShowQuestion;
