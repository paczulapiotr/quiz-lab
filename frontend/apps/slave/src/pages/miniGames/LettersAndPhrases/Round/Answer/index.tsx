import Component from "./Component";
import { useLetters } from "@repo/ui/hooks/miniGames/LettersAndPhrases/useLetters";
import { LettersAndPhrasesInteractions } from "@repo/ui/minigames/actions";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { useCallback, useEffect, useRef } from "react";
import throttle from "lodash/throttle";
import { useGame } from "@repo/ui/contexts/GameContext";
import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";

const Answer = () => {
  const canAnswer = useRef(true);
  const { mutateAsync } = useSendPlayerInteraction();
  const { gameId, miniGameState: state } = useGame<LettersState, LettersDefinition>();
  const { data: score, refetch } = useGetScore(gameId);
  const { incorrectLetters, phrase, usedLetters, yourTurn } = useLetters();
  const answerCount =
    state?.rounds.find((x) => x.roundId === state.currentRoundId)?.answers
      .length ?? 0;

  useEffect(() => {
    if (yourTurn) {
      refetch();
    }
  }, [refetch, yourTurn]);

  const handleSelect = useCallback(
    async (selected: string) => {
      if (usedLetters.includes(selected.toLowerCase())) return;

      const letter = selected.toLowerCase();
      if (!canAnswer.current || usedLetters.includes(letter)) return;

      const setCanAnswer = throttle(
        (can: boolean) => {
          canAnswer.current = can;
        },
        1000,
        { trailing: true },
      );

      setCanAnswer(false);
      await mutateAsync({
        gameId: gameId!,
        value: letter,
        interactionType: LettersAndPhrasesInteractions.Answer,
      });
      setCanAnswer(true);
    },
    [gameId, mutateAsync, usedLetters],
  );

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      onSelect={handleSelect}
      incorrectLetters={incorrectLetters}
      phrase={phrase}
      usedLetters={usedLetters}
      yourTurn={yourTurn}
      timerKey={answerCount}
    />
  );
};

export default Answer;
