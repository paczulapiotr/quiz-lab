import QueryKeys from "@/api/QueryKeys";
import { getAppliedPowerPlay } from "@/api/requests/minigames/abcd";
import { useQuery } from "react-query";

export const useGetAppliedPowerPlay = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getAppliedPowerPlay(gameId!),
    queryKey: [QueryKeys.MINIGAMES.ABCD.APPLIED_POWER_PLAYS, gameId],
  });
};
