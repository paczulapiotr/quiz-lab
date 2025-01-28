import { useMutation } from "react-query";
import { AxiosError, AxiosResponse } from "axios";
import {
  sendPlayerInteraction,
  SendPlayerInteractionRequest,
} from "../requests/miniGame";

export const useSendPlayerInteraction = () => {
  return useMutation<AxiosResponse, AxiosError, SendPlayerInteractionRequest>({
    mutationFn: (req) => sendPlayerInteraction(req),
  });
};
