import QueryKeys from "@/api/QueryKeys";
import { getQuestion } from "@/api/requests/minigames/abcd";
import { useQuery } from "react-query";

export const useGetQuestion = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getQuestion(gameId!),
    queryKey: [QueryKeys.MINIGAMES.ABCD.QUESTION, gameId],
  });
};
