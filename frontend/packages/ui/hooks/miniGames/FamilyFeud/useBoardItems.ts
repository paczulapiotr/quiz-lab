import { useMemo } from "react";
import {
  FamilyFeudDefinition,
  FamilyFeudState,
} from "@repo/ui/api/queries/minigames/familyFeud";
import { Answer } from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useGame } from "../../../contexts/GameContext";

export const useBoardItems = (
  showLastAnswer: boolean = false
) => {
  const {
    miniGameState: state,
    miniGameDefinition: definition,
    you,
  } = useGame<FamilyFeudState, FamilyFeudDefinition>();

  return useMemo(() => {
    const roundState = state?.rounds.find(
      (x) => x.roundId == state?.currentRoundId
    );
    const lastAns = roundState?.answers.at(-1);

    const roundDef = definition?.rounds.find(
      (x) => x.id == state?.currentRoundId
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
      currentPlayerId: state?.currentGuessingPlayerId,
      youAreGuessing: state?.currentGuessingPlayerId === you?.id,
    };
  }, [
    definition?.rounds,
    showLastAnswer,
    state?.currentGuessingPlayerId,
    state?.currentRoundId,
    state?.rounds,
    you?.id,
  ]);
};
