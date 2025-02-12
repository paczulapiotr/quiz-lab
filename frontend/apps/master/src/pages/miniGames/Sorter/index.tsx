import { useParams } from "react-router";
import { SyncReceiveData } from "@repo/ui/services/types";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { SorterActions } from "@repo/ui/minigames/actions";
import { useCallback } from "react";
type Props = {
  basePath: string;
};

const Sorter = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();

  return (
    <PageTemplate squares>
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={useCallback((message) => {
          switch (message.action) {
            case SorterActions.RoundStart:
              return "/sort_prepare";
            default:
              return "";
          }
        }, [])}
        routes={{
          "/sort_prepare": <h1>{`TODO: ${gameId}`}</h1>,
        }}
      />
    </PageTemplate>
  );
};

export default Sorter;
