import TutorialVideo from "@/pages/temp/TutorialVideo";
import { useLocalSync } from "@repo/ui/hooks";
import { useEffect } from "react";
import { useParams } from "react-router";

const Component = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { sendSync } = useLocalSync();

  useEffect(() => {
    const timeout = setTimeout(
      () =>
        sendSync("MiniGameUpdate", {
          gameId: gameId!,
          action: "PowerPlayExplainStop",
          miniGameType: "AbcdWithCategories",
        }),
      10_000,
    );

    return () => clearTimeout(timeout);
  }, [gameId, sendSync]);

  return <TutorialVideo title="Prezentacja zagrywek..." />;
};

export default Component;
