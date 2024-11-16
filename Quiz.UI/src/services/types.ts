//#region utility types
export type SyncSendCallback<T extends SyncSendDefinitionNames> = (
  data: SyncSendData[T],
) => void;

export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (
  data: SyncReceiveData[T],
) => void;
//#endregion

// Extendable
export type SyncSendDefinitionNames = "Ping" | "SelectAnswer";
export type SyncReceiveDefinitionNames = "Pong" | "SelectAnswer";

export interface SyncSendData {
  Ping: {
    Message: string;
    Amount: number;
  };
  SelectAnswer: {
    Answer: string;
  };
}

export interface SyncReceiveData {
  Pong: {
    Message: string;
    Amount: number;
  };
  SelectAnswer: {
    Answer: string;
  };
}
