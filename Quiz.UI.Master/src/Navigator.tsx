import { PropsWithChildren } from "react";
import { useLocalSyncConsumer } from "./hooks/useLocalSyncConsumer";
import { useNavigate } from "react-router";
import { GameStatus } from "./services/types";

const Navigator = ({ children }: PropsWithChildren) => {
  const navigate = useNavigate();

  useLocalSyncConsumer("GameStatusUpdate", (message) => {
    switch (message?.Status) {
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
  });

  return children;
};

export default Navigator;
