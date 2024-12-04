import { PropsWithChildren } from "react";
import { useNavigate } from "react-router";
import { GameStatus } from "./services/types";
import { useLocalSyncConsumer } from "quiz-common-ui";

const Navigator = ({ children }: PropsWithChildren) => {
  const navigate = useNavigate();

  useLocalSyncConsumer(
    "GameStatusUpdate",
    (message) => {
      console.log("GameStatusUpdate", message);
      switch (message?.Status) {
        case GameStatus.GameCreated:
        case GameStatus.GameJoined:
          navigate(`/join/${message.GameId}/`);
          break;
        case GameStatus.GameStarting:
          navigate(`/starting/${message.GameId}/`);
          break;
        case GameStatus.RulesExplaining:
          navigate(`/rules/${message.GameId}/`);
          break;
        case GameStatus.RoundStarting:
          navigate(`/round/${message.GameId}/`);
          break;

        case GameStatus.RoundEnding:
          navigate(`/round/end/${message.GameId}/`);
          break;

        case GameStatus.GameEnding:
          navigate(`/end/${message.GameId}/`);
          break;
        default:
          break;
      }
    },
    true,
  );

  return children;
};

export default Navigator;
