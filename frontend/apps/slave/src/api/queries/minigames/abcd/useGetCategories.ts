import QueryKeys from "@/api/QueryKeys";
import { getCategories } from "@/api/requests/minigames/abcd";
import { useQuery } from "react-query";

export const useGetCategories = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getCategories(gameId!),
    queryKey: [QueryKeys.MINIGAMES.ABCD.CATEGORIES, gameId],
  });
};
