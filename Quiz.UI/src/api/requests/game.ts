import { AxiosResponse } from "axios";
import instance from "../instance";

export type JoinGameRequest = { playerName: string };

export const joinGame = async (playerName: string) =>
  await instance.post<never, never, JoinGameRequest>("/game/join", {
    playerName,
  });

export type GetCurrentGameResponse = {
  gameId: string;
  gameSize: number;
  playerNames: string[];
};

export const getCreatedGame = async () =>
  (
    await instance.get<never, AxiosResponse<GetCurrentGameResponse>>(
      "/game/current",
    )
  ).data;
