import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import { Tile } from "@repo/ui/components";

type Props = {
  gameId?: string;
};

const RoundEnd = ({ gameId }: Props) => {
  const { answers, question } = useBoardItems(gameId);

  return (
    <>
      <MainBoard answers={answers} question={question} />
      <div style={{ marginTop: "auto" }}>
        <Tile text="KONIEC RUNDY" blue />
      </div>
    </>
  );
};

export default RoundEnd;
