import { useLocalSyncService } from "./useLocalSyncService";

export const useLocalSync = () => {
  const { sendSync } = useLocalSyncService();
  return { sendSync };
};
