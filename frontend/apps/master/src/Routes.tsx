import { JoinGame } from "./pages/JoinGame";
import { RulesExplaining } from "./pages/RulesExplaining";
import { MiniGameEnding } from "./pages/MiniGameEnding";
import { GameEnding } from "./pages/GameEnding";
import { MiniGameStarting } from "./pages/MiniGameStarting";
import { MiniGameStarted } from "./pages/MiniGameStarted";
import { GameNavigator } from "@repo/ui/components";
import { GameStatus } from "@repo/ui/services/types";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";
import { useEffect } from "react";
import { getGame } from "@repo/ui/api/requests/game";
import { useParams } from "react-router";

const Routes = () => {
  const { gameId } = useParams<{ gameId: string }>();
  const { players, setPlayers } = usePlayers();

  useEffect(() => {
    if (gameId && players.length === 0) {
      getGame(gameId).then((game) => {
        setPlayers(game.players);
      });
    }
  }, [gameId, players, setPlayers]);

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
        [GameStatus.GameEnded]: <GameEnding ended />,
      }}
    />
  );
};

export default Routes;
