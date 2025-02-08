import Component from "./Component";
import { useParams } from "react-router";
import { useLetters } from "@repo/ui/hooks/miniGames/LettersAndPhrases/useLetters";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";
import { useCallback } from "react";

const RefreshOnActions = [
  LettersAndPhrasesActions.AnswerStart,
  LettersAndPhrasesActions.Answered,
];

type Props = {
  startSeconds?: number;
  onTimeUp?: () => void;
};

const Round = ({ onTimeUp, startSeconds }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data, refetch } = useGetMiniGame<LettersState, LettersDefinition>(
    gameId,
  );
  const { players } = usePlayers();
  const {incorrectLetters, usedLetters, phrase, timestamp} = useLetters(gameId, RefreshOnActions);
  
  useLocalSyncConsumer("MiniGameNotification", useCallback((message) => {
    if (
      message?.gameId === gameId &&
      message?.action != null &&
      [
        LettersAndPhrasesActions.AnswerStart,
        LettersAndPhrasesActions.Answered,
      ].includes(message.action)
    ) {
      refetch();
    }
  },[gameId, refetch]));

  return (
    <Component
      phrase={phrase}
      usedLetters={usedLetters}
      onTimeUp={onTimeUp}
      startSeconds={startSeconds}
      incorrectLetters={incorrectLetters}
      playerAnswering={players.find(
        (x) => x.id === data?.state?.currentGuessingPlayerId,
      )?.name}
      timerKey={timestamp}
    />
  );
};

export default Round;
