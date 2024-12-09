import { useEffect, useRef } from "react";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../services/types";
import { useLocalSyncService } from "./useLocalSyncService";

export const useLocalSyncConsumer = <T extends SyncReceiveDefinitionNames>(
  name: T,
  key: string,
  callback: SyncReceiveCallback<T>,
) => {
  const { onSync, offSync } = useLocalSyncService();
  const callbackRef = useRef<SyncReceiveCallback<T>>();
  useEffect(() => {
    callbackRef.current = callback;
    onSync(name, callback, key);
    return () => {
      if (callbackRef.current) {
        offSync(name, key);
      }
    };
  }, [callback, key, name, offSync, onSync]);
};
