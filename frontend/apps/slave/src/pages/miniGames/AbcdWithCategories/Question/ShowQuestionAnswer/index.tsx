import Component from "./Component";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowQuestionAnswer = () => {
  const {
    you,
    miniGameState: state,
    miniGameDefinition: definition,
  } = useGame<AbcdState, AbcdDefinition>();
  const answers = state?.rounds.find(
    (r) => r.roundId === state?.currentRoundId,
  )?.answers;
  const selected = answers?.find((x) => x.playerId === you?.id);
  const answerDefs =
    definition?.rounds
      .find((x) => x.id === state?.currentRoundId)
      ?.categories.find((x) => x.id === state?.currentCategoryId)
      ?.questions.find((x) => x.id === state?.currentQuestionId)?.answers ?? [];

  const totalScore =
    state?.rounds.reduce(
      (acc, round) =>
        acc + (round.answers.find((a) => a.playerId === you?.id)?.points ?? 0),
      0,
    ) ?? 0;

  return (
    <Component
      answerId={selected?.answerId}
      answerScore={selected?.points ?? 0}
      score={totalScore}
      answers={answerDefs}
    />
  );
};

export default ShowQuestionAnswer;
