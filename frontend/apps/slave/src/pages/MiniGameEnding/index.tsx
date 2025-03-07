import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { PageTemplate, CenteredInstruction } from "@repo/ui/components";
import { useGame } from "@repo/ui/contexts/GameContext";

const MiniGameEnding = () => {
  const { gameId } = useGame();
  const { data } = useGetScore(gameId);

  return (
    <PageTemplate key={"mini_game_ending"}>
      <CenteredInstruction
        title={`Zdobyte punkty: ${data?.miniGameScore}`}
        secondaryText="Podsumowanie rundy"
      />
    </PageTemplate>
  );
};

export default MiniGameEnding;
