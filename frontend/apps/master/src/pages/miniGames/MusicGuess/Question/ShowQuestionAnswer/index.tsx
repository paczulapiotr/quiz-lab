import Component from "./Component";
import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import { useMemo } from "react";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowQuestionAnswer = () => {
  const {
    players,
    miniGameState: state,
    miniGameDefinition: definition,
  } = useGame<MusicGuessState, MusicGuessDefinition>();

  const playersData = useMemo(
    () =>
      state?.rounds
        .find((round) => round.roundId === state?.currentRoundId)
        ?.answers.map((x) => ({
          id: x.playerId,
          name: players.find((p) => p.id === x.playerId)?.name ?? "",
          answerId: x.answerId ?? undefined,
          answerPoints: x.points,
          roundPoints:
            state?.rounds.reduce(
              (acc, round) =>
                acc +
                (round.answers.find((a) => a.playerId === x.playerId)?.points ??
                  0),
              0,
            ) ?? 0,
        })) ?? [],
    [state?.currentRoundId, state?.rounds, players],
  );

  const answers = useMemo(
    () =>
      definition?.rounds
        .find((x) => x.id === state?.currentRoundId)
        ?.categories.find((x) => x.id === state?.currentCategoryId)
        ?.questions.find((x) => x.id === state?.currentQuestionId)?.answers ??
      [],
    [
      definition?.rounds,
      state?.currentCategoryId,
      state?.currentQuestionId,
      state?.currentRoundId,
    ],
  );

  return <Component answers={answers ?? []} players={playersData} />;
};

export default ShowQuestionAnswer;
