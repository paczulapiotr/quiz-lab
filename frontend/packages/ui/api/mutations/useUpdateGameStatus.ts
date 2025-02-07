import { useMutation } from "react-query";
import { updateGameStatus, UpdateGameStatusRequest } from "../requests/game";
import { AxiosError } from "axios";

export const useUpdateGameStatus = () => {
  return useMutation<never, AxiosError, UpdateGameStatusRequest>({
    mutationFn: (req) => updateGameStatus(req),
  });
};
