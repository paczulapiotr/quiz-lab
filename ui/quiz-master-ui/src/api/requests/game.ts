import { AxiosResponse } from "axios";
import instance from "../instance";

export type JoinGameRequest = { playerName: string; gameId: string };

export const joinGame = async (playerName: string, gameId: string) =>
  await instance.post<never, never, JoinGameRequest>("/game/join", {
    playerName,
    gameId,
  });

export type GetCurrentGameResponse = {
  gameId: string;
  gameSize: number;
  playerNames: string[];
  yourName?: string;
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

export type PlayerScore = {
  playerName: string;
  playerDeviceId: string;
  miniGameScore: number;
  totalScore: number;
};

export type GetScoresResponse = {
  miniGameId: string;
  miniGameType: number;
  playerScores: PlayerScore[];
} & PlayerScore;

export const getScores = async (gameId: string) =>
  (
    await instance.get<never, AxiosResponse<GetScoresResponse>>(
      `/game/${gameId}/mini-game/score`,
    )
  ).data;
