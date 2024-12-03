import { useQuery } from "react-query";
import { getGame } from "../requests/game";
import QueryKeys from "../QueryKeys";

export const useGetGame = (gameId?: string) => {
  return useQuery({
    enabled: Boolean(gameId),
    queryFn: () => getGame(gameId!),
    queryKey: [QueryKeys.GAME, gameId],
  });
};
