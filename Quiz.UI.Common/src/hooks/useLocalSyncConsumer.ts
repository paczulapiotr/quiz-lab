import { useEffect } from "react";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../services/types";
import { useLocalSyncService } from "./useLocalSyncService";

export const useLocalSyncConsumer = <T extends SyncReceiveDefinitionNames>(
  name: T,
  callback: SyncReceiveCallback<T>,
) => {
  const { onSync, offSync } = useLocalSyncService();

  useEffect(() => {
    onSync(name, callback);
    return () => {
      offSync(name);
    };
  }, [callback, name, offSync, onSync]);
};
