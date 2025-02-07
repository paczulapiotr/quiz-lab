import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const AnswerQuestion = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const question = data?.definition?.rounds
    .find((round) => round.id === data?.state?.currentRoundId)
    ?.categories.find(
      (category) => category.id === data?.state?.currentCategoryId,
    )
    ?.questions.find(
      (question) => question.id === data?.state?.currentQuestionId,
    );

  return (
    <Component
      answers={question?.answers ?? []}
      question={question?.text ?? ""}
    />
  );
};

export default AnswerQuestion;
