export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (
  data?: SyncReceiveData[T],
) => void;

// Extendable

export type SyncSendDefinitionNames =
  | "Ping"
  | "SelectAnswer"
  | "GameStatusUpdate"
  | "MiniGameUpdate";

export type SyncReceiveDefinitionNames =
  | "Pong"
  | "SelectAnswer"
  | "GameStatusUpdate"
  | "MiniGameNotification";

export interface SyncSendData {
  Ping: {
    message: string;
    amount: number;
  };
  SelectAnswer: {
    answer: string;
  };
  GameStatusUpdate: {
    gameId: string;
    status: GameStatus;
  };
  MiniGameUpdate: {
    gameId: string;
    miniGameType: string;
    action: string;
    valujes?: string;
    data?: Record<string, string>;
  };
}

export interface SyncReceiveData {
  Pong: undefined;
  SelectAnswer: {
    answer: string;
  };
  GameStatusUpdate: {
    gameId: string;
    value?: string;
    status: GameStatus;
  };
  MiniGameNotification: {
    gameId: string;
    miniGameType: string;
    action: string;
    metadata?: Record<string, string>;
  };
}

export enum GameStatus {
  GameCreated = 1,
  GameJoined,
  GameStarting,
  GameStarted,
  RulesExplaining,
  RulesExplained,
  MiniGameStarting,
  MiniGameStarted,
  MiniGameEnding,
  MiniGameEnded,
  GameEnding,
  GameEnded,
}

export const GameStatusNames: Record<GameStatus, string> = {
  [GameStatus.GameCreated]: "GameCreated",
  [GameStatus.GameJoined]: "GameJoined",
  [GameStatus.GameStarting]: "GameStarting",
  [GameStatus.GameStarted]: "GameStarted",
  [GameStatus.RulesExplaining]: "RulesExplaining",
  [GameStatus.RulesExplained]: "RulesExplained",
  [GameStatus.MiniGameStarting]: "MiniGameStarting",
  [GameStatus.MiniGameStarted]: "MiniGameStarted",
  [GameStatus.MiniGameEnding]: "MiniGameEnding",
  [GameStatus.MiniGameEnded]: "MiniGameEnded",
  [GameStatus.GameEnding]: "GameEnding",
  [GameStatus.GameEnded]: "GameEnded",
};
