import { useParams } from "react-router";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { SorterActions } from "@repo/ui/minigames/actions";
import Sorting from "./Sorting";

type Props = {
  basePath: string;
};

const Sorter = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  console.log("Sorter", gameId);
  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case SorterActions.RoundStart:
              return "/sort_prepare";
            default:
              return "";
          }
        }}
        routes={{
          "/sort_prepare": <Sorting />,
        }}
      />
    </PageTemplate>
  );
};

export default Sorter;
