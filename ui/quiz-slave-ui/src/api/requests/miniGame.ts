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

export type SendPlayerInteractionRequest = {
  gameId: string;
  interactionType: string;
  value?: string;
  data?: Record<string, string>;
};

export const sendPlayerInteraction = async ({
  gameId,
  ...data
}: SendPlayerInteractionRequest): Promise<AxiosResponse> =>
  await instance.post<never, AxiosResponse>(
    `/game/${gameId}/mini-game/interaction`,
    {
      data,
    },
  );
