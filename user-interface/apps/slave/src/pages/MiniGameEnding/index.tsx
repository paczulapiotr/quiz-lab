import { useGetScore } from "@/api/queries/useGetScore";
import { PageTemplate, CenteredInstruction } from "@repo/ui/components";
import { useParams } from "react-router";

const MiniGameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data } = useGetScore(gameId);

  return (
    <PageTemplate>
      <CenteredInstruction
        title={`Zdobyte punkty: ${data?.miniGameScore}`}
        secondaryText="Podsumowanie rundy"
      />
    </PageTemplate>
  );
};

export default MiniGameEnding;
