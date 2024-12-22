import { useCallback, useState } from "react";
import { PageTemplate } from "@/components/PageTemplate";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile } from "quiz-common-ui/components";
import AdminPanel from "@/AdminPanel";

const AbcdWithCategories = () => {
  const [status, setStatus] = useState("");
  useLocalSyncConsumer(
    "MiniGameNotification",
    "AbcdWithCategories",
    useCallback((data) => {
      setStatus(`${data?.miniGameType}/${data?.action}`);
    }, []),
  );

  return (
    <PageTemplate>
      <Tile text={status ?? "N/A"} blue />
      <AdminPanel />
    </PageTemplate>
  );
};

export default AbcdWithCategories;
