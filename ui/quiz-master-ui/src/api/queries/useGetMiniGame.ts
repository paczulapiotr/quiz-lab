import { useQuery } from "react-query";
import { getMiniGame } from "../requests/miniGame";
import QueryKeys from "../QueryKeys";

export const useGetMiniGame = (gameId?: string) => {
  return useQuery({
    enabled: Boolean(gameId),
    queryFn: () => getMiniGame(gameId!),
    queryKey: [QueryKeys.CURRENT_MINI_GAME, gameId],
  });
};
