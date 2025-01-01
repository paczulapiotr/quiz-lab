import QueryKeys from "@/api/QueryKeys";
import { getSelectedCategory } from "@/api/requests/minigames/abcd";
import { useQuery } from "react-query";

export const useGetSelectedCategory = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getSelectedCategory(gameId!),
    queryKey: [QueryKeys.MINIGAMES.ABCD.SELECTED_CATEGORY, gameId],
  });
};
