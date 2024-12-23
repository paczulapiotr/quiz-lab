import { GameStatus } from "quiz-common-ui";
import { GameNavigator } from "quiz-common-ui/components";
import { JoinGame } from "./pages/JoinGame";
import RulesExplaining from "./pages/RulesExplaining";
import GameEnding from "./pages/GameEnding";
import MiniGameStarting from "./pages/MiniGameStarting";
import MiniGameEnding from "./pages/MiniGameEnding";
import MiniGameStarted from "./pages/MiniGameStarted";

const Routes = () => {
  return (
    <GameNavigator
      basePath=""
      pages={{
        [GameStatus.GameCreated]: <JoinGame />,
        [GameStatus.GameJoined]: <JoinGame />,
        [GameStatus.GameStarting]: <JoinGame starting />,
        [GameStatus.RulesExplaining]: <RulesExplaining />,
        [GameStatus.MiniGameStarting]: <MiniGameStarting />,
        [GameStatus.MiniGameStarted]: (
          <MiniGameStarted
            basePath="minigame_play"
            miniGameType="AbcdWithCategories"
          />
        ),
        [GameStatus.MiniGameEnding]: <MiniGameEnding />,
        [GameStatus.GameEnding]: <GameEnding />,
      }}
    />
  );
};

export default Routes;
