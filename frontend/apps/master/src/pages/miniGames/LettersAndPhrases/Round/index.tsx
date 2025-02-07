import Component from "./Component";
import { useParams } from "react-router";
import { LettersAndPhrasesActions } from "@repo/ui/minigames/actions";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import {
  LettersDefinition,
  LettersState,
} from "@repo/ui/api/queries/minigames/letters";
import uniq from "lodash/uniq";

type Props = {
  startSeconds?: number;
  onTimeUp?: () => void;
};

const Round = ({ onTimeUp, startSeconds }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data, refetch } = useGetMiniGame<LettersState, LettersDefinition>(
    gameId,
  );

  useLocalSyncConsumer("MiniGameNotification", (message) => {
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
  });

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

  return (
    <Component
      phrase={phrase}
      usedLetters={usedLetters}
      onTimeUp={onTimeUp}
      startSeconds={startSeconds}
      incorrectLetters={incorrectLetters}
    />
  );
};

export default Round;
