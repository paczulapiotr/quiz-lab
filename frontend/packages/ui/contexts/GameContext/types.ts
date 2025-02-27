import { GameStatus } from "../../services/types";

export type GameContextType = {
  you?: Player;
  players: Player[];
  gameStatus?: GameStatus;
  miniGameStatus?: string;
  miniGameDefinition?: object;
  miniGameState?: object;
};

export type Player = {
  id: string;
  name: string;
  deviceId: string;
};
