import Component from "./Component";
import { useLetters } from "../useLetters";
import { useParams } from "react-router";
import {
  LettersAndPhrasesActions,
  LettersAndPhrasesInteractions,
} from "@repo/ui/minigames/actions";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";

const Answer = () => {
  const { mutateAsync } = useSendPlayerInteraction();
  const { gameId } = useParams<{ gameId: string }>();
  const { data: score } = useGetScore(gameId);
  const { incorrectLetters, phrase, usedLetters, yourTurn } = useLetters(
    gameId,
    [LettersAndPhrasesActions.AnswerStart, LettersAndPhrasesActions.Answered],
  );
  const handleSelect = async (selected: string) => {
    await mutateAsync({
      gameId: gameId!,
      value: selected,
      interactionType: LettersAndPhrasesInteractions.Answer,
    });
  };
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
