import { PropsWithChildren, useEffect, useRef, useState } from "react";
import { LocalSyncService } from "../../services/LocalSyncService";
import { LocalSyncServiceContext } from "./context";

export const LocalSyncServiceProvider = ({ children }: PropsWithChildren) => {
  const [connected, setConnected] = useState(false);
  const localSyncService = useRef<LocalSyncService>(
    new LocalSyncService(
      () => {
        setConnected(true);
        console.log("Connected to LocalSyncService");
      },
      () => {
        setConnected(false);
        console.log("Disconnected from LocalSyncService");
      },
    ),
  );

  useEffect(() => {
    const syncService = localSyncService.current;
    syncService.init();

    return () => {
      syncService.dispose();
    };
  }, [localSyncService]);

  return (
    <LocalSyncServiceContext.Provider
      value={{
        addOnConnect: localSyncService.current.addOnConnectedCallback,
        addOnDisconnect: localSyncService.current.addOnDisconnectedCallback,
        removeOnConnect: localSyncService.current.removeOnConnectedCallback,
        removeOnDisconnect:
          localSyncService.current.removeOnDisconnectedCallback,
        connected,
        offSync: localSyncService.current.offSync,
        onSync: localSyncService.current.onSync,
        sendSync: localSyncService.current.sendSync,
      }}
    >
      {children}
    </LocalSyncServiceContext.Provider>
  );
};
