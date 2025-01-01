import { useGetScore } from "@/api/queries/useGetScore";
import { CenteredInstruction, PageTemplate, ScoreTile } from "@repo/ui/components";

import { useParams } from "react-router";

const GameEnding = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data } = useGetScore(gameId);

  return (
    <PageTemplate>
      <ScoreTile score={data?.totalScore} />
      <CenteredInstruction
        title="Podsumowanie gry"
        secondaryText="Koniec gry"
      />
    </PageTemplate>
  );
};

export default GameEnding;
