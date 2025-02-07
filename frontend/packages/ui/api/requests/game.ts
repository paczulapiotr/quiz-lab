import { AxiosResponse } from "axios";
import instance from "../instance";
import { GameStatus } from "@repo/ui";

export type GetCurrentGameResponse = {
  gameId: string;
  gameSize: number;
  players: { id: string; name: string; deviceId: string }[];
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

export type UpdateGameStatusRequest = { status: GameStatus; gameId: string };

export const updateGameStatus = async ({
  gameId,
  status,
}: UpdateGameStatusRequest) =>
  await instance.post<never, never, Pick<UpdateGameStatusRequest, "status">>(
    `/game/${gameId}/status`,
    { status },
  );

export type JoinGameRequest = { playerName: string; gameId: string };
export type JoinGameResponse = { ok: boolean; errorCode?: string };

export const joinGame = async (playerName: string, gameId: string) =>
  (
    await instance.post<
      never,
      AxiosResponse<JoinGameResponse>,
      JoinGameRequest
    >("/game/join", {
      playerName,
      gameId,
    })
  ).data;
