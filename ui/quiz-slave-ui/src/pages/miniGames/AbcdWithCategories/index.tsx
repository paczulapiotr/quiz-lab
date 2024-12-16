import { useCallback, useState } from "react";
import { PageTemplate } from "@/components/PageTemplate";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { Tile } from "quiz-common-ui/components";

const AbcdWithCategories = () => {
  const [status,setStatus] = useState("");

  useLocalSyncConsumer(
    "MiniGameNotification",
    "AbcdWithCategories",
    useCallback(
        (data) => {
            setStatus(`${data?.miniGameType}/${data?.action}`)
      },[]
    ),
  );


  return (
    <PageTemplate>
          <Tile text={status ?? "N/A"} blue />
    </PageTemplate>
  );
};

export default AbcdWithCategories;

