import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import uniq from "lodash/uniq";
import { useMemo } from "react";
import { useGame } from "../../../contexts/GameContext";

export const useLetters = () => {
  const {
    miniGameDefinition: definition,
    miniGameState: state,
    you,
  } = useGame<LettersState, LettersDefinition>();

  return useMemo(() => {
    const round = definition?.rounds.find(
      (x) => x.id === state?.currentRoundId
    );
    const roundState = state?.rounds.find(
      (x) => x.roundId === state?.currentRoundId
    );
    const phrase = round?.phrase.split(" ") ?? [];
    const usedLetters = uniq(roundState?.answers.map((a) => a.letter) ?? []);
    const incorrectLetters = uniq(
      roundState?.answers
        .filter((x) => !x.isCorrect || x.letter == null)
        .map((a) => a.letter) ?? []
    );
    const yourTurn = state?.currentGuessingPlayerId === you?.id;

    return { phrase, usedLetters, incorrectLetters, yourTurn };
  }, [
    definition?.rounds,
    state?.currentGuessingPlayerId,
    state?.currentRoundId,
    state?.rounds,
    you?.id,
  ]);
};
