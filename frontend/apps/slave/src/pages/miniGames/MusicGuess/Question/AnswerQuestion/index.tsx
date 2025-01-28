import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { MusicGuessInteractions } from "@repo/ui/minigames/actions";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { MusicGuessState, MusicGuessDefinition } from "@repo/ui/api/queries/minigames/musicGuess";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(gameId);
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const state = data?.state;
  const def = data?.definition;
  const question = def?.rounds
    .find((r) => r.id === state?.currentRoundId)?.categories
    .find((c) => c.id === state?.currentCategoryId)?.questions
    .find((q) => q.id === state?.currentQuestionId);

  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: MusicGuessInteractions.QuestionAnswer, value });

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question?.answers ?? []}
      question={question?.text}
      onAnswer={answer}
    />
  );
};

export default AnswerQuestion;
