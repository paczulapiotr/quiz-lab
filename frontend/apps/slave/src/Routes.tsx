import { JoinGame } from "./pages/JoinGame";
import RulesExplaining from "./pages/RulesExplaining";
import GameEnding from "./pages/GameEnding";
import MiniGameStarting from "./pages/MiniGameStarting";
import MiniGameEnding from "./pages/MiniGameEnding";
import MiniGameStarted from "./pages/MiniGameStarted";
import { Animate } from "@repo/ui/components";
import { GameStatus } from "@repo/ui";
import { useGame } from "@repo/ui/contexts/GameContext";
import { Welcome } from "./pages/Welcome";

const Routes = () => {
  const { gameStatus, gameId } = useGame();
  return <Animate>{gameId == null ? <Welcome /> : render(gameStatus)}</Animate>;
};

export default Routes;

const render = (gameStatus: GameStatus | undefined) => {
  switch (gameStatus) {
    case GameStatus.GameCreated:
      return <JoinGame key={GameStatus.GameCreated} />;
    case GameStatus.GameStarting:
      return <JoinGame starting key={GameStatus.GameCreated} />;
    case GameStatus.RulesExplaining:
      return <RulesExplaining key={gameStatus} />;
    case GameStatus.MiniGameStarting:
      return <MiniGameStarting key={gameStatus} />;
    case GameStatus.MiniGameStarted:
      return <MiniGameStarted key={gameStatus} />;
    case GameStatus.MiniGameEnding:
      return <MiniGameEnding key={gameStatus} />;
    case GameStatus.GameEnding:
      return <GameEnding key={gameStatus} />;
    case GameStatus.GameEnded:
      return <GameEnding key={gameStatus} />;
    default:
      return null;
  }
};
