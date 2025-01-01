import { useEffect, useMemo, useRef } from "react";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../services/types";
import { useLocalSyncService } from "./useLocalSyncService";
import { v4 as uuid } from "uuid";

export const useLocalSyncConsumer = <T extends SyncReceiveDefinitionNames>(
  name: T,
  callback: SyncReceiveCallback<T>
) => {
  const key = useMemo(() => uuid(), []);
  const { onSync, offSync } = useLocalSyncService();
  const callbackRef = useRef<SyncReceiveCallback<T>>();

  useEffect(() => {
    callbackRef.current = callback;
    console.log("onSync", name, key);
    onSync(name, callback, key);
    return () => {
      if (callbackRef.current) {
        console.log("offSync", name, key);
        offSync(name, key);
      }
    };
  }, [callback, key, name, offSync, onSync]);
};
