import { GameStatus } from "@repo/ui";
import { useCallback, useMemo } from "react";
import { Route, Routes } from "react-router";
import GenericNavigator from "./GenericNavigator";
import { SyncReceiveData } from "@/services/types";
import { cleanupSlash } from "@/utility";

type Props = {
  basePath: string;
  pages: {
    [GameStatus.GameCreated]: JSX.Element;
    [GameStatus.GameJoined]: JSX.Element;
    [GameStatus.GameStarting]: JSX.Element;
    [GameStatus.RulesExplaining]: JSX.Element;
    [GameStatus.MiniGameStarting]: JSX.Element;
    [GameStatus.MiniGameStarted]: JSX.Element;
    [GameStatus.MiniGameEnding]: JSX.Element;
    [GameStatus.GameEnding]: JSX.Element;
  };
};

const GameNavigator = ({ pages, basePath }: Props) => {
  const createNavigationPath = useCallback(
    (message: SyncReceiveData["GameStatusUpdate"]) => {
      switch (message?.status) {
        case GameStatus.GameCreated:
          return `${message.gameId}/created/`;
        case GameStatus.GameJoined:
          return `${message.gameId}/joined/`;
        case GameStatus.GameStarting:
          return `${message.gameId}/starting/`;
        case GameStatus.RulesExplaining:
          return `${message.gameId}/rules/`;
        case GameStatus.MiniGameStarting:
          return `${message.gameId}/minigame/`;
        case GameStatus.MiniGameStarted:
          return `${message.gameId}/minigame_play/`;
        case GameStatus.MiniGameEnding:
          return `${message.gameId}/minigame_finish/`;
        case GameStatus.GameEnding:
          return `${message.gameId}/end/`;
        default:
          return "";
      }
    },
    [],
  );
  const routes = useMemo<Record<string, JSX.Element>>(
    () => ({
      ":gameId/created/*": pages[GameStatus.GameCreated],
      ":gameId/join/*": pages[GameStatus.GameJoined],
      ":gameId/starting/*": pages[GameStatus.GameStarting],
      ":gameId/rules/*": pages[GameStatus.RulesExplaining],
      ":gameId/minigame/*": pages[GameStatus.MiniGameStarting],
      ":gameId/minigame_play/*": pages[GameStatus.MiniGameStarted],
      ":gameId/minigame_finish/*": pages[GameStatus.MiniGameEnding],
      ":gameId/end/*": pages[GameStatus.GameEnding],
    }),
    [pages],
  );

  return (
    <Routes>
      <Route
        path="game/*"
        element={
          <GenericNavigator<SyncReceiveData["GameStatusUpdate"]>
            basePath={cleanupSlash(basePath + "/game")}
            identifier={"GameNavigator"}
            routes={routes}
            queueName={"GameStatusUpdate"}
            createNavigationPath={createNavigationPath}
          />
        }
      />
    </Routes>
  );
};

export default GameNavigator;
