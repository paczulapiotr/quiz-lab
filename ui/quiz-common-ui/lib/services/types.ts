export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (
  data?: SyncReceiveData[T],
) => void;

// Extendable

export type SyncSendDefinitionNames =
  | "Ping"
  | "SelectAnswer"
  | "GameStatusUpdate";

export type SyncReceiveDefinitionNames =
  | "Pong"
  | "SelectAnswer"
  | "GameStatusUpdate";

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
}

export interface SyncReceiveData {
  Pong: undefined;
  SelectAnswer: {
    answer: string;
  };
  GameStatusUpdate: {
    gameId: string;
    status: GameStatus;
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
