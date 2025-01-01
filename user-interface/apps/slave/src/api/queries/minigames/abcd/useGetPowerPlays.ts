import QueryKeys from "@/api/QueryKeys";
import { getPowerPlays } from "@/api/requests/minigames/abcd";
import { useQuery } from "react-query";

export const useGetPowerPlays = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getPowerPlays(gameId!),
    queryKey: [QueryKeys.MINIGAMES.ABCD.POWER_PLAYS, gameId],
  });
};
