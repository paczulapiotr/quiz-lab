import { JoinGame } from "./pages/JoinGame";
import RulesExplaining from "./pages/RulesExplaining";
import GameEnding from "./pages/GameEnding";
import MiniGameStarting from "./pages/MiniGameStarting";
import MiniGameEnding from "./pages/MiniGameEnding";
import MiniGameStarted from "./pages/MiniGameStarted";
import { GameNavigator } from "@repo/ui/components";
import { GameStatus } from "@repo/ui";

const Routes = () => {
  return (
    <GameNavigator
      basePath=""
      pages={{
        [GameStatus.GameCreated]: <JoinGame />,
        [GameStatus.GameStarting]: <JoinGame starting />,
        [GameStatus.RulesExplaining]: <RulesExplaining />,
        [GameStatus.MiniGameStarting]: <MiniGameStarting />,
        [GameStatus.MiniGameStarted]: (
          <MiniGameStarted basePath="minigame_play" />
        ),
        [GameStatus.MiniGameEnding]: <MiniGameEnding />,
        [GameStatus.GameEnding]: <GameEnding />,
        [GameStatus.GameEnded]: <GameEnding />,
      }}
    />
  );
};

export default Routes;
