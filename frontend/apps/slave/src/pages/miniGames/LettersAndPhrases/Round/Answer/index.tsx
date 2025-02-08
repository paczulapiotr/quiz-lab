import Component from "./Component";
import { useLetters } from "../useLetters";
import { useParams } from "react-router";
import {
  LettersAndPhrasesActions,
  LettersAndPhrasesInteractions,
} from "@repo/ui/minigames/actions";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import { useCallback } from "react";

const RefreshOnActions = [LettersAndPhrasesActions.AnswerStart, LettersAndPhrasesActions.Answered];

const Answer = () => {
  const { mutateAsync } = useSendPlayerInteraction();
  const { gameId } = useParams<{ gameId: string }>();
  const { data: score } = useGetScore(gameId);
  const { incorrectLetters, phrase, usedLetters, yourTurn } = useLetters(
    gameId,
    RefreshOnActions,
  );

  const handleSelect = useCallback(async (selected: string) => {
    const letter = selected.toLowerCase();
    if (usedLetters.includes(letter)) return;
    
    await mutateAsync({
      gameId: gameId!,
      value: letter,
      interactionType: LettersAndPhrasesInteractions.Answer,
    });
  }, [gameId, mutateAsync, usedLetters]);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      onSelect={handleSelect}
      incorrectLetters={incorrectLetters}
      phrase={phrase}
      usedLetters={usedLetters}
      yourTurn={yourTurn}
    />
  );
};

export default Answer;
