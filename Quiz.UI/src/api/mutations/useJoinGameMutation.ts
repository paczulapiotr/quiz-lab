import { useMutation } from "react-query";
import { joinGame, JoinGameRequest } from "../requests/game";
import { AxiosError } from "axios";

export const useJoinGameMutation = () => {
  return useMutation<never, AxiosError, JoinGameRequest>({
    mutationFn: ({ playerName }) => joinGame(playerName),
  });
};
