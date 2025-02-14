import { useParams } from "react-router";
import { PageTemplate, GenericNavigator } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { SorterActions } from "@repo/ui/minigames/actions";
import Sorting from "./Sorting";
import Summary from "./Summary";

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
        createNavigationPath={(message) => {
          switch (message.action) {
            case SorterActions.RoundStart:
              return "/start";
            case SorterActions.RoundStarted:
              return "/started";
            case SorterActions.RoundEnd:
              return "/end";
            case SorterActions.RoundSummary:
              return "/ended";
            default:
              return "";
          }
        }}
        routes={{
          "/start": <Sorting gameId={gameId} />,
          "/started": <Sorting gameId={gameId} started />,
          "/end": <Summary gameId={gameId} />,
          "/ended": <Summary gameId={gameId} finished/>,
        }}
      />
    </PageTemplate>
  );
};

export default Sorter;
