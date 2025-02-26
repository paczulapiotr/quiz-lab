import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
  useCallback,
} from "react";
import { getGame } from "@repo/ui/api/requests/game";
import { useParams } from "react-router";
interface Player {
  id: string;
  name: string;
  deviceId: string;
}

interface PlayersContextProps {
  players: Player[];
  you?: Player;
  setPlayers: (you: Player | undefined, players: Player[]) => void;
}

const PlayersContext = createContext<PlayersContextProps | undefined>(
  undefined
);

export const PlayersProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [players, setPlayers] = useState<Player[]>([]);
  const [you, setYou] = useState<Player>();
  const _setPlayers = useCallback(
    (_you: Player | undefined, _players: Player[] | undefined) => {
      setYou(_you);
      setPlayers(_players ?? []);
    },
    []
  );

  return (
    <PlayersContext.Provider value={{ you, players, setPlayers: _setPlayers }}>
      {children}
    </PlayersContext.Provider>
  );
};

export const usePlayers = (): {
  you: Player | undefined;
  players: Player[];
  reload: () => Promise<void>;
} => {
  const { gameId } = useParams<{ gameId: string }>();
  const context = useContext(PlayersContext);
  if (!context) {
    throw new Error("usePlayers must be used within a PlayersProvider");
  }

  const players = context.players;
  const setPlayers = context.setPlayers;

  const reload = useCallback(async () => {
    if (gameId == null) return;

    const game = await getGame(gameId);
    setPlayers(
      game.players.find((x) => x.deviceId === game.yourDeviceId),
      game.players
    );
  }, [gameId, setPlayers]);

  useEffect(() => {
    if (gameId && players?.length === 0) {
      reload();
    }
  }, [gameId, players?.length, reload]);

  return { you: context.you, players: context.players, reload };
};
