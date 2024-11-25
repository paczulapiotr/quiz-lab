import instance from "./instance";

export type JoinGameRequest = { playerName: string; gameId: string };
export type JoinGameResponse = { players: string[]; slots: number };

export const joinGame = (playerName: string, gameId: string) =>
  instance.post<never, JoinGameResponse, JoinGameRequest>("/game/join", {
    playerName,
    gameId,
  });
