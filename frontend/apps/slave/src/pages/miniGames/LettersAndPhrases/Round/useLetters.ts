import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import uniq from "lodash/uniq";
import { useMemo } from "react";

export const useLetters = (gameId?: string, refreshOnActions?: string[]) => {
  const { data, refetch } = useGetMiniGame<LettersState, LettersDefinition>(
    gameId,
  );

  useLocalSyncConsumer("MiniGameNotification", (message) => {
    if (
      (refreshOnActions ?? []).length > 0 &&
      message?.gameId === gameId &&
      message?.action != null &&
      refreshOnActions?.includes(message.action)
    ) {
      refetch();
    }
  });

  return useMemo(() => {
    const round = data?.definition?.rounds.find(
      (x) => x.id === data?.state?.currentRoundId,
    );
    const roundState = data?.state?.rounds.find(
      (x) => x.roundId === data?.state?.currentRoundId,
    );
    const phrase = round?.phrase.split(" ") ?? [];
    const usedLetters = roundState?.answers.map((a) => a.letter) ?? [];
    const incorrectLetters = uniq(
      roundState?.answers
        .filter((x) => !x.isCorrect || x.letter == null)
        .map((a) => a.letter) ?? [],
    );
    const yourTurn = data?.state?.currentGuessingPlayerId === data?.playerId;

    return { phrase, usedLetters, incorrectLetters, yourTurn };
  }, [
    data?.definition?.rounds,
    data?.playerId,
    data?.state?.currentGuessingPlayerId,
    data?.state?.currentRoundId,
    data?.state?.rounds,
  ]);
};
