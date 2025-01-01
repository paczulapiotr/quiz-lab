import { AxiosResponse } from "axios";
import instance from "../instance";

export type GetCurrentMiniGameResponse = {
  miniGameId: string;
  miniGameType: number;
};

export const getMiniGame = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetCurrentMiniGameResponse>>(
      `/game/${gameId}/mini-game`,
    )
  ).data;
