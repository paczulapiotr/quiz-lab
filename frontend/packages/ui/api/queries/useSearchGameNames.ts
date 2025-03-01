import { useQuery } from "react-query";
import { searchGameNames } from "../requests/game";
import QueryKeys from "../QueryKeys";

export const useSearchGameNames = () => {
  return useQuery({
    queryFn: () => searchGameNames(),
    queryKey: [QueryKeys.GAME_NAMES],
  });
};
