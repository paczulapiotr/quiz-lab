import { useMutation, useQueryClient } from "react-query";
import { joinGame, JoinGameRequest } from "../requests/game";
import { AxiosError } from "axios";
import QueryKeys from "../QueryKeys";

export const useJoinGameMutation = () => {
  const queryClient = useQueryClient();

  return useMutation<never, AxiosError, JoinGameRequest>({
    mutationFn: ({ playerName, gameId }) => joinGame(playerName, gameId),
    onSuccess: () => {
      queryClient.invalidateQueries(QueryKeys.GAME);
    },
  });
};
