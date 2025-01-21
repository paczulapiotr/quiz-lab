import { AxiosResponse } from "axios";
import instance from "../instance";
import { MiniGameType } from "./minigames/types";

export type GetCurrentMiniGameResponse = {
  miniGameId: string;
  miniGameType: MiniGameType;
};

export const getMiniGame = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetCurrentMiniGameResponse>>(
      `/game/${gameId}/mini-game`,
    )
  ).data;

export type UpdateMiniGameRequest = {
  action: string;
  gameId: string;
};

export const updateMiniGame = async (
  { gameId, action }: UpdateMiniGameRequest) => (
  await instance.post<never, never, Pick<UpdateMiniGameRequest, "action">>(
    `/game/${gameId}/mini-game/update`,
    {
      action
    },
  ));
