import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const answer = (value: string) =>
    sendAsync({
      gameId,
      interactionType: AbcdInteractions.QuestionAnswer,
      value,
    });

  const roundState = data?.state?.rounds.find(
    (round) => round.roundId === data?.state?.currentRoundId);
  
  const roundDef = data?.definition?.rounds.find(
    (round) => round.id === data?.state?.currentRoundId);
  
  const question = roundDef?.categories
    .find((category) => category.id === data?.state?.currentCategoryId)
    ?.questions.find(
      (question) => question.id === data?.state?.currentQuestionId,
  );
  
  const powerPlays = (roundState?.powerPlays[data?.playerId ?? ""] ?? []).map(x => x.powerPlay);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question?.answers ?? []}
      question={question?.text ?? ""}
      onAnswer={answer}
      powerPlays={powerPlays}
    />
  );
};

export default AnswerQuestion;
