import Component from "./Component";
import { useLetters } from "@repo/ui/hooks/miniGames/LettersAndPhrases/useLetters";
import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import { useGame } from "@repo/ui/contexts/GameContext";

type Props = {
  startSeconds?: number;
  onTimeUp?: () => void;
};

const Round = ({ onTimeUp, startSeconds }: Props) => {
  const { players, miniGameState: state } = useGame<
    LettersState,
    LettersDefinition
  >();
  const { incorrectLetters, usedLetters, phrase } = useLetters();
  const answerCount =
    state?.rounds.find((x) => x.roundId === state.currentRoundId)?.answers
      .length ?? 0;

  return (
    <Component
      phrase={phrase}
      usedLetters={usedLetters}
      onTimeUp={onTimeUp}
      startSeconds={startSeconds}
      incorrectLetters={incorrectLetters}
      playerAnswering={
        players.find((x) => x.id === state?.currentGuessingPlayerId)?.name
      }
      timerKey={answerCount}
    />
  );
};

export default Round;
