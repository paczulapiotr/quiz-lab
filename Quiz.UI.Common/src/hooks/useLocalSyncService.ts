import { LocalSyncServiceContext } from "../contexts/LocalSyncServiceContext/context";
import { useContext } from "react";

export const useLocalSyncService = () => {
  const context = useContext(LocalSyncServiceContext);
  if (context === undefined) {
    throw new Error(
      "useLocalSyncService must be used within a LocalSyncServiceProvider",
    );
  }
  return context;
};
