import { useQuery } from "react-query";
import { getCreatedGame } from "../requests/game";
import QueryKeys from "../QueryKeys";

export const useGetCreatedGame = () => {
  return useQuery({
    queryFn: getCreatedGame,
    queryKey: [QueryKeys.CURRENT_GAME],
  });
};
