import { useMutation } from "react-query";
import { updateMiniGame,UpdateMiniGameRequest} from "../requests/miniGame";
import { AxiosError } from "axios";

export const useUpdateMiniGame = () => {

  return useMutation<never, AxiosError, UpdateMiniGameRequest>({
    mutationFn: (req) => updateMiniGame(req),
  });
};
