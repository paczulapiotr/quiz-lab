import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { AbcdState, AbcdDefinition } from "@repo/ui/api/queries/minigames/abcd";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const answers = data?.state?.rounds.find((r) => r.roundId === data?.state?.currentRoundId)?.answers
  const selected = answers?.find(x => x.playerId === data?.playerId);
  const answerDefs = data?.definition?.rounds
    .find(x => x.id === data.state?.currentRoundId)?.categories
    .find(x => x.id === data.state?.currentCategoryId)?.questions
    .find(x => x.id === data.state?.currentQuestionId)?.answers ?? [];
  
  const totalScore = data?.state?.rounds
    .reduce((acc, round) => acc + (round.answers.find(a => a.playerId === data?.playerId)?.points ?? 0), 0) ?? 0;

  return (
    <Component
      answerId={selected?.answerId}
      answerScore={selected?.points ?? 0}
      score={totalScore}
      answers={answerDefs}
    />
  );
};

export default ShowQuestionAnswer;
