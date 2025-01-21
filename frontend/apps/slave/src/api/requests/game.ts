import { AxiosResponse } from "axios";
import instance from "../instance";

export type JoinGameRequest = { playerName: string; gameId: string };
export type JoinGameResponse = { ok: boolean, errorCode?: string };

export const joinGame = async (playerName: string, gameId: string) =>
  (await instance.post<never, AxiosResponse<JoinGameResponse>, JoinGameRequest>("/game/join", {
    playerName,
    gameId,
  })).data;

export type GetCurrentGameResponse = {
  gameId: string;
  gameSize: number;
  playerNames: string[];
  yourPlayerName?: string;
  yourDeviceId?: string;
  isStarted: boolean;
  isFinished: boolean;
};

export const getGame = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetCurrentGameResponse>>(
      `/game/${gameId}`,
    )
  ).data;

export type GetScoresResponse = {
  miniGameId: string;
  miniGameType: number;
  playerName: string;
  playerDeviceId: string;
  miniGameScore: number;
  totalScore: number;
  players: {
    playerName: string;
    playerDeviceId: string;
    miniGameScore: number;
    totalScore: number;
  }[];
};

export const getScores = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetScoresResponse>>(
      `/game/${gameId}/mini-game/score`,
    )
  ).data;
