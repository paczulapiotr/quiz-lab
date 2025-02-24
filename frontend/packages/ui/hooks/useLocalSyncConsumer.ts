import { useEffect, useMemo } from "react";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../services/types";
import { useLocalSyncService } from "./useLocalSyncService";
import { v4 as uuid } from "uuid";

export const useLocalSyncConsumer = <T extends SyncReceiveDefinitionNames>(
  name: T,
  callback: SyncReceiveCallback<T>,
  silent: boolean = false,
) => {
  const key = useMemo(() => uuid(), []);
  const { onSync, offSync } = useLocalSyncService();

  useEffect(() => {
    console.debug("onSync", name, key);
    onSync(name, callback, key, silent);
    return () => {
        console.debug("offSync", name, key);
        offSync(name, key);
    };
  }, [callback, key, name, offSync, onSync, silent]);
};
