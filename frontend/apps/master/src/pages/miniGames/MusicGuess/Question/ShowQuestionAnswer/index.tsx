import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  MusicGuessState,
  MusicGuessDefinition,
} from "@repo/ui/api/queries/minigames/musicGuess";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";
import { useMemo } from "react";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(
    gameId,
  );
  const { players } = usePlayers();

  const playersData = useMemo(
    () =>
      data?.state?.rounds
        .find((round) => round.roundId === data?.state?.currentRoundId)
        ?.answers.map((x) => ({
          id: x.playerId,
          name: players.find((p) => p.id === x.playerId)?.name ?? "",
          answerId: x.answerId ?? undefined,
          answerPoints: x.points,
          roundPoints:
            data.state?.rounds.reduce(
              (acc, round) =>
                acc +
                (round.answers.find((a) => a.playerId === x.playerId)?.points ??
                  0),
              0,
            ) ?? 0,
        })) ?? [],
    [data?.state?.currentRoundId, data?.state?.rounds, players],
  );

  const answers = useMemo(
    () =>
      data?.definition?.rounds
        .find((x) => x.id === data.state?.currentRoundId)
        ?.categories.find((x) => x.id === data.state?.currentCategoryId)
        ?.questions.find((x) => x.id === data.state?.currentQuestionId)
        ?.answers ?? [],
    [
      data?.definition?.rounds,
      data?.state?.currentCategoryId,
      data?.state?.currentQuestionId,
      data?.state?.currentRoundId,
    ],
  );

  return <Component answers={answers ?? []} players={playersData} />;
};

export default ShowQuestionAnswer;
