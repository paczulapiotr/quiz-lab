import { useCallback, useState } from "react";
import { PageTemplate } from "@/components/PageTemplate";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { GenericNavigator, Tile } from "quiz-common-ui/components";
import { useGetMiniGame } from "@/api/queries/useGetMiniGame";
import { useParams } from "react-router";
import { SyncReceiveData } from "node_modules/quiz-common-ui/dist/services/types";
import SelectPowerPlay from "./PowerPlays/SelectPowerPlay";
import ShowAppliedPowerPlay from "./PowerPlays/ShowAppliedPowerPlay";
import { PowerPlaysEnum } from "./PowerPlays/types";
import SelectCategory from "./Categories/SelectCategory";
import ShowCategory from "./Categories/ShowCategory";
import ShowQuestion from "./Question/ShowQuestion";
import AnswerQuestion from "./Question/AnswerQuestion";
import ShowQuestionAnswer from "./Question/ShowQuestionAnswer";
type Props = {
  basePath: string;
};
const AbcdWithCategories = ({ basePath }: Props) => {
  const [status, setStatus] = useState("");
  const { gameId } = useParams<{ gameId: string }>();
  const { data, refetch } = useGetMiniGame(gameId);
  console.log("minigame:", data);

  useLocalSyncConsumer(
    "MiniGameNotification",
    "AbcdWithCategories_Debug",
    useCallback(
      (data) => {
        refetch();
        setStatus(`${data?.miniGameType}/${data?.action}`);
      },
      [refetch],
    ),
  );

  return (
    <PageTemplate>
      <Tile text={status ?? "N/A"} blue />
      <GenericNavigator<SyncReceiveData["MiniGameNotification"]>
        basePath={basePath}
        key={"AbcdWithCategories"}
        queueName={"MiniGameNotification"}
        routes={{
          "/powerplay_explain": <p>EXPLAIN POWER PLAYS</p>,
          "/powerplay_select": <SelectPowerPlay gameId={gameId!} />,
          "/powerplay_apply": (
            <ShowAppliedPowerPlay
              appliedPowerPlays={[
                {
                  playerId: "test_player",
                  playerName: "Test Player",
                  powerPlay: PowerPlaysEnum.Freeze,
                },
              ]}
            />
          ),
          "/category_select": <SelectCategory gameId={gameId!} />,
          "/category_show": <ShowCategory gameId={gameId!} />,
          "/question_show": <ShowQuestion gameId={gameId!} />,
          "/question_answer": <AnswerQuestion gameId={gameId!} />,
          "/question_answer_show": <ShowQuestionAnswer gameId={gameId!} />,
        }}
        createNavigationPath={(message) => {
          switch (message.action) {
            case "PowerPlayExplainStart":
            case "PowerPlayExplainStop":
              return "/powerplay_explain";
            case "PowerPlayStart":
              return "/powerplay_select";
            case "PowerPlayApplyStart":
            case "PowerPlayApplyStop":
              return "/powerplay_apply";
            case "CategorySelectStart":
              return "/category_select";
            case "CategoryShowStart":
            case "CategoryShowStop":
              return "/category_show";
            case "QuestionShowStart":
            case "QuestionShowStop":
              return "/question_show";
            case "QuestionAnswerStart":
              return "/question_answer";
            case "QuestionAnswerShowStart":
            case "QuestionAnswerShowStop":
              return "/question_answer_show";
            default:
              return "";
          }
        }}
      />
    </PageTemplate>
  );
};

export default AbcdWithCategories;
