import { useCallback, useEffect, useState } from "react";
import { PageTemplate } from "@/components/PageTemplate";
import { useLocalSync, useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile } from "quiz-common-ui/components";
import { useParams } from "react-router";
import { GameStatus } from "quiz-common-ui";

const AbcdWithCategories = () => {
  const [status, setStatus] = useState("");
  const { sendSync } = useLocalSync();
  const { gameId } = useParams<{ gameId: string }>();

  useLocalSyncConsumer(
    "MiniGameNotification",
    "AbcdWithCategories",
    useCallback((data) => {
      setStatus(`${data?.miniGameType}/${data?.action}`);
    }, []),
  );

  useEffect(() => {
    const timer = setTimeout(() => {
      sendSync("GameStatusUpdate", {
        gameId: gameId!,
        status: GameStatus.MiniGameStarted,
      });
    }, 10_000);

    return () => clearTimeout(timer);
  }, [gameId, sendSync]);

  return (
    <PageTemplate>
      <Tile text={status ?? "N/A"} blue />
    </PageTemplate>
  );
};

export default AbcdWithCategories;
