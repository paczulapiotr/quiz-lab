import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { MusicGuessState, MusicGuessDefinition } from "@repo/ui/api/queries/minigames/musicGuess";
import Component from "./Component";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const {data} = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(gameId);
  const def = data?.definition;
  const state = data?.state
  const question = def?.rounds
    .find((r) => r.id === state?.currentRoundId)?.categories
    .find((c) => c.id === state?.currentCategoryId)?.questions
    .find((q) => q.id === state?.currentQuestionId);

  return (
    <Component
      answers={question?.answers ?? []}
      question={question?.text}
      questionAudio={question?.audioUrl}
    />
  );
};

export default AnswerQuestion;
