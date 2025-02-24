import { useParams } from "react-router";
import { PageTemplate, GenericNavigator, ScoreTile } from "@repo/ui/components";
import { SyncReceiveData } from "@repo/ui/services/types";
import { FamilyFeudActions } from "@repo/ui/minigames/actions";
import ShowQuestion from "./ShowQuestion";
import AnswerQuestion from "./AnswerQuestion";
import ShowAnswer from "./ShowAnswer";
import RoundEnd from "./RoundEnd";
import { useGetScore } from "@repo/ui/api/queries/useGetScore";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { useCallback } from "react";

type Props = {
  basePath: string;
};

const FamilyFeud = ({ basePath }: Props) => {
  const { gameId } = useParams<{ gameId: string }>();
  const { data, refetch } = useGetScore(gameId);

  useLocalSyncConsumer("MiniGameNotification", useCallback((message) => {
    if (message?.action === FamilyFeudActions.Answered) {
      refetch();
    }
  },[refetch]), true);

  return (
    <PageTemplate squares>
      <ScoreTile score={data?.miniGameScore} />
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        disableAnimation
        basePath={basePath}
        queueName={"MiniGameNotification"}
        createNavigationPath={(message) => {
          switch (message.action) {
            case FamilyFeudActions.QuestionShow:
            case FamilyFeudActions.QuestionShown:
              return "/question";
            case FamilyFeudActions.AnswerStart:
            case FamilyFeudActions.Answered:
              return "/answer";
            case FamilyFeudActions.AnswerShow:
            case FamilyFeudActions.AnswerShown:
              return "/answer_show";
            case FamilyFeudActions.RoundEnd:
            case FamilyFeudActions.RoundEnded:
              return "/end";
            default:
              return "";
          }
        }}
        routes={{
          "/question": <ShowQuestion gameId={gameId} />,
          "/answer": <AnswerQuestion gameId={gameId} />,
          "/answer_show": <ShowAnswer gameId={gameId} />,
          "/end": <RoundEnd gameId={gameId} />,
        }}
      />
    </PageTemplate>
  );
};

export default FamilyFeud;
