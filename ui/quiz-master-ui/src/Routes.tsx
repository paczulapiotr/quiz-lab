import { GameStatus } from "quiz-common-ui";
import { GameNavigator } from "quiz-common-ui/components";
import { JoinGame } from "./pages/JoinGame";
import { RulesExplaining } from "./pages/RulesExplaining";
import { MiniGameEnding } from "./pages/MiniGameEnding";
import { GameEnding } from "./pages/GameEnding";
import AbcdWithCategories from "./pages/miniGames/AbcdWithCategories";
import { MiniGameStarting } from "./pages/MiniGameStarting";

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
        [GameStatus.MiniGameStarted]: <AbcdWithCategories />,
        [GameStatus.MiniGameEnding]: <MiniGameEnding />,
        [GameStatus.GameEnding]: <GameEnding />,
      }}
    />
  );
};

export default Routes;
