import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import { getGame } from "@repo/ui/api/requests/game";
import { useParams } from "react-router";
interface Player {
  id: string;
  name: string;
}

interface PlayersContextProps {
  players: Player[];
  setPlayers: (players: Player[]) => void;
}

const PlayersContext = createContext<PlayersContextProps | undefined>(
  undefined
);

export const PlayersProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [players, setPlayers] = useState<Player[]>([]);

  return (
    <PlayersContext.Provider value={{ players, setPlayers }}>
      {children}
    </PlayersContext.Provider>
  );
};

export const usePlayers = (): PlayersContextProps => {
  const { gameId } = useParams<{ gameId: string }>();
  const context = useContext(PlayersContext);
  if (!context) {
    throw new Error("usePlayers must be used within a PlayersProvider");
  }

  const players = context.players;
  const setPlayers = context.setPlayers;

  useEffect(() => {
    if (gameId && players.length === 0) {
      getGame(gameId).then((game) => {
        setPlayers(game.players);
      });
    }
  }, [gameId, players.length, setPlayers]);

  return context;
};
