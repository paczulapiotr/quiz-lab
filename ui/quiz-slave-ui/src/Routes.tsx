import { GameStatus } from "quiz-common-ui";
import { GameNavigator } from "quiz-common-ui/components";
import { JoinGame } from "./pages/JoinGame";
import GameStarting from "./pages/GameStarting";
import RulesExplaining from "./pages/RulesExplaining";
import GameEnding from "./pages/GameEnding";
import MiniGameStarting from "./pages/MiniGameStarting";
import MiniGameEnding from "./pages/MiniGameEnding";
import MiniGameStarted from "./pages/MiniGameStarted";
import { useLocation } from "react-router";

const Routes = () => {
  const { pathname } = useLocation();
  console.log(pathname);
  return (
    <GameNavigator
      basePath=""
      pages={{
        [GameStatus.GameCreated]: <JoinGame />,
        [GameStatus.GameJoined]: <JoinGame />,
        [GameStatus.GameStarting]: <GameStarting />,
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
