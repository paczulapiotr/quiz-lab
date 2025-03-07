import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import {
  CenteredInstruction,
  PageTemplate,
  ScoreTile,
} from "@repo/ui/components";
import { useGame } from "@repo/ui/contexts/GameContext";

const GameEnding = () => {
  const { gameId } = useGame();
  const { data } = useGetScore(gameId);

  return (
    <PageTemplate key={"game_ending"}>
      <ScoreTile score={data?.totalScore} />
      <CenteredInstruction
        title="Podsumowanie gry"
        secondaryText="Koniec gry"
      />
    </PageTemplate>
  );
};

export default GameEnding;
