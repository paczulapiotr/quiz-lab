export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (
  data?: SyncReceiveData[T],
) => void;

// Extendable

export type SyncSendDefinitionNames = "Ping" | "SelectAnswer";

export type SyncReceiveDefinitionNames =
  | "Pong"
  | "SelectAnswer"
  | "GameCreated"
  | "PlayerJoined"
  | "GameStatusUpdate";

export interface SyncSendData {
  Ping: {
    message: string;
    amount: number;
  };
  SelectAnswer: {
    answer: string;
  };
}

export interface SyncReceiveData {
  Pong: undefined;
  SelectAnswer: {
    answer: string;
  };
  GameCreated: {
    gameId: string;
    gameSize: number;
  };
  PlayerJoined: {
    deviceId: string;
    playerName: string;
  };
  GameStatusUpdate: {
    gameId: string;
    status: GameStatus;
  };
}

export enum GameStatus {
  GameCreated = 0,
  GameJoined,
  GameStarting,
  GameStarted,
  RulesExplaining,
  RulesExplained,
  RoundStarting,
  RoundStarted,
  RoundEnding,
  RoundEnded,
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
  [GameStatus.RoundStarting]: "RoundStarting",
  [GameStatus.RoundStarted]: "RoundStarted",
  [GameStatus.RoundEnding]: "RoundEnding",
  [GameStatus.RoundEnded]: "RoundEnded",
  [GameStatus.GameEnding]: "GameEnding",
  [GameStatus.GameEnded]: "GameEnded",
};
