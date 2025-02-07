import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const ShowQuestion = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { data: score } = useGetScore(gameId!);

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
      score={score?.miniGameScore ?? 0}
      question={question?.text ?? ""}
      questionId={question?.id ?? ""}
    />
  );
};

export default ShowQuestion;
