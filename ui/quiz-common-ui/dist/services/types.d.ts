export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (data?: SyncReceiveData[T]) => void;
export type SyncSendDefinitionNames = "Ping" | "SelectAnswer" | "GameStatusUpdate" | "MiniGameUpdate";
export type SyncReceiveDefinitionNames = "Pong" | "SelectAnswer" | "GameStatusUpdate" | "MiniGameNotification";
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
        status: GameStatus;
    };
    MiniGameNotification: {
        gameId: string;
        miniGameType: string;
        action: string;
        metadata?: Record<string, string>;
    };
}
export declare enum GameStatus {
    GameCreated = 1,
    GameJoined = 2,
    GameStarting = 3,
    GameStarted = 4,
    RulesExplaining = 5,
    RulesExplained = 6,
    MiniGameStarting = 7,
    MiniGameStarted = 8,
    MiniGameEnding = 9,
    MiniGameEnded = 10,
    GameEnding = 11,
    GameEnded = 12
}
export declare const GameStatusNames: Record<GameStatus, string>;
