import { useSendPlayerInteraction } from "@repo/ui/api/mutations/useSendPlayerInteraction";
import Component from "./Component";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { AbcdInteractions } from "@repo/ui/minigames/actions";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { PowerPlaysEnum } from "../../PowerPlays/types";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const {data} = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { data: score } = useGetScore(gameId);
  const { mutateAsync: sendAsync } = useSendPlayerInteraction();

  const answer = (value: string) =>
    sendAsync({ gameId, interactionType: AbcdInteractions.QuestionAnswer, value });

  const question = data?.definition?.rounds.find((round) => round.id === data?.state?.currentRoundId)?.categories
    .find((category) => category.id === data?.state?.currentCategoryId)?.questions
    .find((question) => question.id === data?.state?.currentQuestionId);

  return (
    <Component
      score={score?.miniGameScore ?? 0}
      answers={question?.answers ?? []}
      question={question?.text ?? ""}
      onAnswer={answer}
      powerPlays={[PowerPlaysEnum.Bombs, PowerPlaysEnum.Freeze, PowerPlaysEnum.Letters, PowerPlaysEnum.Slime]}
    />
  );
};

export default AnswerQuestion;
