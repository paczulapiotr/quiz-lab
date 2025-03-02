import Component from "./Component";
import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowQuestionAnswer = () => {
  const {
    miniGameDefinition: definition,
    miniGameState: state,
    you,
  } = useGame<MusicGuessState, MusicGuessDefinition>();
  const definitionRound = definition?.rounds.find(
    (x) => x.id === state?.currentRoundId,
  );

  const answers =
    definitionRound?.categories
      .find((x) => x.id === state?.currentCategoryId)
      ?.questions.find((x) => x.id === state?.currentQuestionId)?.answers ?? [];

  const answer = state?.rounds
    .find((round) => round.roundId === state?.currentRoundId)
    ?.answers.find((x) => x.playerId === you?.id);

  const totalScore =
    state?.rounds.reduce(
      (acc, round) =>
        acc +
        (round.answers.find((a) => a.playerId === you?.id)?.points ?? 0),
      0,
    ) ?? 0;

  return (
    <Component
      answerId={answer?.answerId ?? undefined}
      answerScore={answer?.points ?? 0}
      score={totalScore}
      answers={answers ?? []}
    />
  );
};

export default ShowQuestionAnswer;
