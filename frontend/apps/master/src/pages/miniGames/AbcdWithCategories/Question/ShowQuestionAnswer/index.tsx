import Component from "./Component";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowQuestionAnswer = () => {
  const { players, miniGameState: state, miniGameDefinition: definition } = useGame<AbcdState, AbcdDefinition>();

  const answers = state?.rounds.find(
    (r) => r.roundId === state?.currentRoundId,
  )?.answers;
  const answerDefs =
    definition?.rounds
      .find((x) => x.id === state?.currentRoundId)
      ?.categories.find((x) => x.id === state?.currentCategoryId)
      ?.questions.find((x) => x.id === state?.currentQuestionId)
      ?.answers ?? [];

  const playersData = players.map((player) => ({
    id: player.id,
    name: player.name,
    answerId: answers?.find((x) => x.playerId === player.id)?.answerId,
    answerPoints: answers?.find((x) => x.playerId === player.id)?.points ?? 0,
    roundPoints:
      state?.rounds.reduce(
        (acc, round) =>
          acc +
          (round.answers.find((a) => a.playerId === player.id)?.points ?? 0),
        0,
      ) ?? 0,
  }));

  return <Component answers={answerDefs} players={playersData} />;
};

export default ShowQuestionAnswer;
