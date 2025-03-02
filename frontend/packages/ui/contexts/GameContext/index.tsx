import {
  createContext,
  PropsWithChildren,
  useContext,
  useEffect,
  useState,
} from "react";
import { GameContextType } from "./types";
import { GameStatus } from "../../services/types";
import { useGetGame } from "../../api/queries/useGetGame";
import { useLocalSyncConsumer } from "../../hooks";
import { useGetMiniGame } from "../../api/queries/useGetMiniGame";
import { useRoom } from "../RoomContext";

export const GameContext = createContext<GameContextType>({
  gameId: undefined,
  you: undefined,
  players: [],
  gameStatus: undefined,
  miniGameType: undefined,
  miniGameStatus: undefined,
  miniGameDefinition: undefined,
  miniGameState: undefined,
});

export const GameProvider = ({ children }: PropsWithChildren) => {
  const [gameId, setGameId] = useState<string>();
  const { room } = useRoom();
  const { data: game, refetch: refetchGame } = useGetGame(gameId);
  const { data: miniGame, refetch: refetchMiniGame } = useGetMiniGame(gameId);

  const miniGameState = miniGame?.state;
  const miniGameStatus = miniGame?.miniGameStatus;
  const miniGameDefinition = miniGame?.definition;
  const miniGameType = miniGame?.miniGameType;
  const gameStatus = game?.gameStatus;
  const players = game?.players ?? [];
  const you = players.find((p) => p.deviceId === room?.uniqueId);

  useLocalSyncConsumer(
    "GameStatusUpdate",
    (message) => {
      if (message?.status === GameStatus.GameCreated) {
        setGameId(message.gameId);
      } else {
        refetchGame();
      }
    },
    false
  );

  useLocalSyncConsumer("MiniGameNotification", () => refetchMiniGame(), false);

  useEffect(() => {
    if (room?.gameId) {
      setGameId(room.gameId);
    }
  }, [room?.gameId]);

  return (
    <GameContext.Provider
      value={{
        gameId,
        players,
        gameStatus,
        miniGameType,
        miniGameDefinition,
        miniGameState,
        miniGameStatus,
        you,
      }}
    >
      {children}
    </GameContext.Provider>
  );
};

export const useGame = <TState = object, TDefinition = object>() => {
  const context = useContext(GameContext);
  if (!context) {
    throw new Error("useGame must be used within a GameProvider");
  }

  return {
    ...context,
    miniGameDefinition: context.miniGameDefinition as TDefinition | undefined,
    miniGameState: context.miniGameState as TState | undefined,
  };
};
