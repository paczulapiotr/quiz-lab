import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { players } = usePlayers();
  const answers = data?.state?.rounds.find(
    (r) => r.roundId === data?.state?.currentRoundId,
  )?.answers;
  const answerDefs =
    data?.definition?.rounds
      .find((x) => x.id === data.state?.currentRoundId)
      ?.categories.find((x) => x.id === data.state?.currentCategoryId)
      ?.questions.find((x) => x.id === data.state?.currentQuestionId)
      ?.answers ?? [];

  const playersData = players.map((player) => ({
    id: player.id,
    name: player.name,
    answerId: answers?.find((x) => x.playerId === player.id)?.answerId,
    answerPoints: answers?.find((x) => x.playerId === player.id)?.points ?? 0,
    roundPoints:
      data?.state?.rounds.reduce(
        (acc, round) =>
          acc +
          (round.answers.find((a) => a.playerId === player.id)?.points ?? 0),
        0,
      ) ?? 0,
  }));

  return <Component answers={answerDefs} players={playersData} />;
};

export default ShowQuestionAnswer;
