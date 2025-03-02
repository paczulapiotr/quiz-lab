import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import { Tile } from "@repo/ui/components";

const RoundEnd = () => {
  const { answers, question } = useBoardItems();
  const showAnswers = answers.map((x) => ({ ...x, show: true }));

  return (
    <>
      <MainBoard answers={showAnswers} question={question} />
      <div style={{ marginTop: "auto" }}>
        <Tile text="KONIEC RUNDY" blue />
      </div>
    </>
  );
};

export default RoundEnd;
