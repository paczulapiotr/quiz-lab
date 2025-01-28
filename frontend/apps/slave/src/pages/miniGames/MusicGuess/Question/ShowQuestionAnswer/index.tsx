import Component from "./Component";
import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import { MusicGuessState, MusicGuessDefinition } from "@repo/ui/api/queries/minigames/musicGuess";

type Props = {
  gameId: string;
};

const ShowQuestionAnswer = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<MusicGuessState, MusicGuessDefinition>(gameId);
  const definition = data?.definition;
  const definitionRound = definition?.rounds.find(x => x.id === data?.state?.currentRoundId);
  
  const answers = definitionRound?.categories.find(x => x.id === data?.state?.currentCategoryId)?.questions
    .find(x=>x.id === data?.state?.currentQuestionId)?.answers ?? [];
  
  const answer = data?.state?.rounds.find((round) => round.roundId === data?.state?.currentRoundId)?.answers
    .find(x => x.playerId === data?.playerId);
  
  const totalScore = data?.state?.rounds.reduce((acc, round) => acc + (round.answers.find(a => a.playerId === data?.playerId)?.points ?? 0), 0) ?? 0;
  
  return (
    <Component
      answerId={answer?.answerId ?? undefined}
      answerScore={answer?.points??0}
      score={totalScore}
      answers={answers ?? []}
    />
  );
};

export default ShowQuestionAnswer;
