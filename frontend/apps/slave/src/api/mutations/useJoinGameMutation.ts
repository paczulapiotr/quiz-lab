import { useMutation, useQueryClient } from "react-query";
import { joinGame, JoinGameRequest, JoinGameResponse } from "../requests/game";
import { AxiosError } from "axios";
import QueryKeys from "../QueryKeys";

export const useJoinGameMutation = () => {
  const queryClient = useQueryClient();

  return useMutation<JoinGameResponse, AxiosError, JoinGameRequest>({
    mutationFn: ({ playerName, gameId }) => joinGame(playerName, gameId),
    onSuccess: () => {
      queryClient.invalidateQueries(QueryKeys.GAME);
    },
  });
};
