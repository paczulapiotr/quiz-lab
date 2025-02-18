import MainBoard from "@repo/ui/components/minigames/FamilyFeud/MainBoard";
import { useBoardItems } from "@repo/ui/hooks/minigames/FamilyFeud/useBoardItems";
import { Tile } from "@repo/ui/components";
import { useUpdateMiniGame } from "@repo/ui/api/mutations/useUpdateMiniGame";
import Times from "@repo/ui/config/times";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import { useEffect } from "react";

type Props = {
  gameId?: string;
};

const RoundEnd = ({ gameId }: Props) => {
  const { answers, question } = useBoardItems(gameId);
  const { mutate } = useUpdateMiniGame();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        mutate({
          gameId: gameId!,
          action: FamilyFeudActions.RoundEnded,
        }),
      Times.FamilyFeud.RoundEndSeconds * 1000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, mutate]);

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
