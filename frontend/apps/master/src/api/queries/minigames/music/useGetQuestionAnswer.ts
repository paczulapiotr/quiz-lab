import QueryKeys from "@/api/QueryKeys";
import { getQuestionAnswer } from "@/api/requests/minigames/music";
import { useQuery } from "react-query";

export const useGetQuestionAnswer = (gameId?: string, enabled?: boolean) => {
  return useQuery({
    enabled: Boolean(gameId) && enabled,
    queryFn: () => getQuestionAnswer(gameId!),
    queryKey: [QueryKeys.MINIGAMES.MUSIC.QUESTION_ANSWER, gameId],
  });
};
