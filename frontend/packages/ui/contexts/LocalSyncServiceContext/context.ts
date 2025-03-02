import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
  SyncSendData,
  SyncSendDefinitionNames,
} from "../../services/types";
import { createContext } from "react";

export type LocalSyncServiceContextType = {
  connected: boolean;
  addOnConnect: (callback: () => void) => void;
  addOnDisconnect: (callback: () => void) => void;
  removeOnConnect: (callback: () => void) => void;
  removeOnDisconnect: (callback: () => void) => void;

  onSync<T extends SyncReceiveDefinitionNames>(
    definitionName: T,
    callback: SyncReceiveCallback<T>,
    key: string,
    silent: boolean
  ): void;

  offSync<T extends SyncReceiveDefinitionNames>(
    definitionName: T,
    key: string,
  ): void;

  sendSync<T extends SyncSendDefinitionNames>(
    definitionName: T,
    data?: SyncSendData[T],
  ): Promise<void>;
};

export const LocalSyncServiceContext = createContext<
  LocalSyncServiceContextType | undefined
>(undefined);
