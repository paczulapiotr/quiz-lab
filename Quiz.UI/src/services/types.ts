//#region utility types
export type SyncSendCallback<T extends SyncSendDefinitionNames> = (
  data: SyncSendData[T],
) => void;

export type SyncReceiveCallback<T extends SyncReceiveDefinitionNames> = (
  data: SyncReceiveData[T],
) => void;
//#endregion

// Extendable
export type SyncSendDefinitionNames = "Ping";
export type SyncReceiveDefinitionNames = "Pong";

export interface SyncSendData {
  Ping: {
    Message: string;
    Amount: number;
  };
}

export interface SyncReceiveData {
  Pong: {
    Message: string;
    Amount: number;
  };
}
