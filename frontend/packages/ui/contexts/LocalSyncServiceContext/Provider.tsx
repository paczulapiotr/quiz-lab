import {
  PropsWithChildren,
  useCallback,
  useEffect,
  useRef,
  useState,
} from "react";
import { LocalSyncServiceContext } from "./context";
import { QueueSyncService } from "../../services/QueueSyncService";
import {
  SyncReceiveCallback,
  SyncReceiveDefinitionNames,
} from "../../services/types";
import { useRoom } from "../RoomContext";

type QueueSyncCallback = {
  callback: SyncReceiveCallback<SyncReceiveDefinitionNames>;
  silent: boolean;
};

const LISTEN_TO_MESSAGES: SyncReceiveDefinitionNames[] = [
  "GameStatusUpdate",
  "MiniGameNotification",
];

export const LocalSyncServiceProvider = ({ children }: PropsWithChildren) => {
  const { room } = useRoom();
  const callbackDict = useRef<
    Record<SyncReceiveDefinitionNames, Record<string, QueueSyncCallback>>
  >({
    GameStatusUpdate: {},
    MiniGameNotification: {},
    Pong: {},
    SelectAnswer: {},
  });

  const [connected, setConnected] = useState(false);
  const queueSyncService = useRef<QueueSyncService>(
    new QueueSyncService(
      `/sync?uniqueId=${room!.uniqueId}`,
      LISTEN_TO_MESSAGES,
      () => {
        setConnected(true);
        console.log("Connected to LocalSyncService");
      },
      () => {
        setConnected(false);
        console.log("Disconnected from LocalSyncService");
      }
    )
  );

  useEffect(() => {
    const syncService = queueSyncService.current;
    syncService.init();

    return () => {
      syncService.dispose();
    };
  }, [queueSyncService]);

  const onSync = useCallback(
    <T extends SyncReceiveDefinitionNames>(
      defName: T,
      useCallback: SyncReceiveCallback<T>,
      key: string,
      silent: boolean = false
    ) => {
      (callbackDict.current[defName][key] as unknown) = {
        callback: useCallback,
        silent,
      };
      useCallback;
      queueSyncService.current.onSync(
        defName,
        Object.values(callbackDict.current[defName])
          .filter((x) => !x.silent)
          .map((x) => x.callback),
        Object.values(callbackDict.current[defName])
          .filter((x) => x.silent)
          .map((x) => x.callback)
      );
    },
    []
  );

  const offSync = useCallback(
    <T extends SyncReceiveDefinitionNames>(defName: T, key: string) => {
      delete callbackDict.current[defName][key];
      queueSyncService.current.onSync(
        defName,
        Object.values(callbackDict.current[defName])
          .filter((x) => !x.silent)
          .map((x) => x.callback),
        Object.values(callbackDict.current[defName])
          .filter((x) => x.silent)
          .map((x) => x.callback)
      );
    },
    []
  );
  return (
    <LocalSyncServiceContext.Provider
      value={{
        addOnConnect: queueSyncService.current.addOnConnectedCallback,
        addOnDisconnect: queueSyncService.current.addOnDisconnectedCallback,
        removeOnConnect: queueSyncService.current.removeOnConnectedCallback,
        removeOnDisconnect:
          queueSyncService.current.removeOnDisconnectedCallback,
        connected,
        onSync,
        offSync,
        sendSync: queueSyncService.current.sendSync,
      }}
    >
      {children}
    </LocalSyncServiceContext.Provider>
  );
};
