import { AxiosResponse } from "axios";
import instance from "../instance";
import { MiniGameType } from "./minigames/types";

export type GetCurrentMiniGameResponse<
  TState extends StateBase,
  TDefinition extends DefinitionBase,
> = {
  miniGameId: string;
  miniGameType: MiniGameType;
  playerName?: string;
  playerId?: string;
  playerDeviceId?: string;
  score?: number;
  state?: TState;
  definition?: TDefinition;
};

export type StateBase = object;
export type DefinitionBase = object;

export const getMiniGame = async <
  S extends StateBase,
  D extends DefinitionBase,
>(
  gameId: string,
) =>
  (
    await instance.get<never, AxiosResponse<GetCurrentMiniGameResponse<S, D>>>(
      `/game/${gameId}/mini-game`,
    )
  ).data;

export type UpdateMiniGameRequest = {
  action: string;
  gameId: string;
};

export const updateMiniGame = async ({
  gameId,
  action,
}: UpdateMiniGameRequest) =>
  await instance.post<never, never, Pick<UpdateMiniGameRequest, "action">>(
    `/game/${gameId}/mini-game/update`,
    {
      action,
    },
  );

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
      ...data,
    },
  );
