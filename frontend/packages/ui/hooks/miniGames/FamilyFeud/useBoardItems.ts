import { useMemo } from "react";
import {
  FamilyFeudDefinition,
  FamilyFeudState,
} from "@repo/ui/api/queries/minigames/familyFeud";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { Answer } from "@repo/ui/components/minigames/FamilyFeud/MainBoard";

export const useBoardItems = (
  gameId?: string,
  showLastAnswer: boolean = false
) => {
  const { data } = useGetMiniGame<FamilyFeudState, FamilyFeudDefinition>(
    gameId
  );

  return useMemo(() => {
    const roundState = data?.state?.rounds.find(
      (x) => x.roundId == data?.state?.currentRoundId
    );
    const lastAns = roundState?.answers.at(-1);

    const roundDef = data?.definition?.rounds.find(
      (x) => x.id == data?.state?.currentRoundId
    );
    const selectedIds = roundState?.answers
      .filter((x) => x.matchedAnswerId !== null)
      .map((x) => x.matchedAnswerId);

    const answers: Answer[] =
      roundDef?.answers.map<Answer>((x) => ({
        id: x.id,
        points: x.points,
        text: x.answer,
        show: selectedIds?.includes(x.id) ?? false,
        highlight: showLastAnswer && lastAns?.matchedAnswerId === x.id,
      })) ?? [];
    return {
      answers,
      question: roundDef?.question ?? "",
      lastWrongAnswer:
        lastAns?.matchedAnswerId == null ? (lastAns?.answer ?? "") : "",
      currentPlayerId: data?.state?.currentGuessingPlayerId,
      youAreGuessing: data?.state?.currentGuessingPlayerId === data?.playerId,
    };
  }, [
    data?.definition?.rounds,
    data?.playerId,
    data?.state?.currentGuessingPlayerId,
    data?.state?.currentRoundId,
    data?.state?.rounds,
    showLastAnswer,
  ]);
};
