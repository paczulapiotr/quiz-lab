import QueryKeys from "@/api/QueryKeys";
import { getSelectedCategory } from "@/api/requests/minigames/music";
import { useQuery } from "react-query";

export const useGetSelectedCategory = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getSelectedCategory(gameId!),
    queryKey: [QueryKeys.MINIGAMES.MUSIC.SELECTED_CATEGORY, gameId],
  });
};
