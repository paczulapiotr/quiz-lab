import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowQuestion = () => {
  const { miniGameState: state, miniGameDefinition: definition } = useGame<
    AbcdState,
    AbcdDefinition
  >();
  const question = definition?.rounds
    .find((round) => round.id === state?.currentRoundId)
    ?.categories.find((category) => category.id === state?.currentCategoryId)
    ?.questions.find((question) => question.id === state?.currentQuestionId);

  return (
    <Component
      audioUrl={question?.audioUrl}
      question={question?.text ?? ""}
      questionId={question?.id ?? ""}
    />
  );
};

export default ShowQuestion;
