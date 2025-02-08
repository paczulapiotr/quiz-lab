import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { SyncReceiveCallback } from "@repo/ui/services/types";
import uniq from "lodash/uniq";
import { useCallback, useMemo, useState } from "react";

export const useLetters = (gameId?: string, refreshOnActions?: string[]) => {
  const [timestamp, setTimestamp] = useState(Date.now());
  const { data, refetch } = useGetMiniGame<LettersState, LettersDefinition>(
    gameId,
  );

  const onGameNotification: SyncReceiveCallback<"MiniGameNotification"> =
  useCallback(
    (message) => {
      if (
        (refreshOnActions ?? []).length > 0 &&
        message?.gameId === gameId &&
        message?.action != null &&
        refreshOnActions?.includes(message.action)
      ) {
        setTimestamp(Date.now());
        refetch();
      }
    },
    [gameId, refetch, refreshOnActions],
  );

  useLocalSyncConsumer("MiniGameNotification", onGameNotification);

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

    return { phrase, usedLetters, incorrectLetters, yourTurn, timestamp };
  }, [data?.definition?.rounds, data?.playerId, data?.state?.currentGuessingPlayerId, data?.state?.currentRoundId, data?.state?.rounds, timestamp]);
};
