import { AxiosResponse } from "axios";
import instance from "../instance";
import { AbcdWithCategoriesState } from "./types/AbcdWithCategories";

export type GetCurrentMiniGameResponse = {
miniGameId: string;
playerName?: string;
playerDeviceId?: string;
score: number;
} & AbcdWithCategoriesState;


export const getMiniGame = async (gameId: string) =>
(
  await instance.get<never, AxiosResponse<GetCurrentMiniGameResponse>>(
    `/game/${gameId}/mini-game`,
  )
).data;
