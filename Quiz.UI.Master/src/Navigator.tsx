import { PropsWithChildren, useCallback } from "react";
import { useNavigate } from "react-router";
import { GameStatus } from "./services/types";
import { useLocalSyncConsumer } from "quiz-common-ui";

const Navigator = ({ children }: PropsWithChildren) => {
  const navigate = useNavigate();

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "Navigator",
    useCallback(
      (message) => {
        switch (message?.status) {
          case GameStatus.GameCreated:
          case GameStatus.GameJoined:
            navigate(`/join/${message.gameId}/`);
            break;
          case GameStatus.GameStarting:
            navigate(`/starting/${message.gameId}/`);
            break;
          case GameStatus.RulesExplaining:
            navigate(`/rules/${message.gameId}/`);
            break;
          case GameStatus.RoundStarting:
            navigate(`/round/${message.gameId}/`);
            break;

          case GameStatus.RoundEnding:
            navigate(`/round/end/${message.gameId}/`);
            break;

          case GameStatus.GameEnding:
            navigate(`/end/${message.gameId}/`);
            break;
          default:
            break;
        }
      },
      [navigate],
    ),
  );

  return children;
};

export default Navigator;
