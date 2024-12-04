import { useEffect } from "react";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../services/types";
import { useLocalSyncService } from "./useLocalSyncService";

export const useLocalSyncConsumer = <T extends SyncReceiveDefinitionNames>(
  name: T,
  callback: SyncReceiveCallback<T>,
  isMainHandler: boolean = false,
) => {
  const { onSync, offSync } = useLocalSyncService();

  useEffect(() => {
    onSync(name, callback);
    return () => {
      if (isMainHandler) {
        offSync(name);
      }
    };
  }, [callback, isMainHandler, name, offSync, onSync]);
};
