import { useQuery } from "react-query";
import { getScores } from "../requests/game";
import QueryKeys from "../QueryKeys";

export const useGetScore = (gameId?: string) => {
  return useQuery({
    enabled: Boolean(gameId),
    queryFn: () => getScores(gameId!),
    queryKey: [QueryKeys.SCORE, gameId],
  });
};
