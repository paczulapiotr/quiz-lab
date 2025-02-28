import { MiniGameType } from "../../api/requests/minigames/types";
import { GameStatus } from "../../services/types";

export type GameContextType = {
  gameId?: string;
  you?: Player;
  players: Player[];
  gameStatus?: GameStatus;
  miniGameType?: MiniGameType;
  miniGameStatus?: string;
  miniGameDefinition?: object;
  miniGameState?: object;
};

export type Player = {
  id: string;
  name: string;
  deviceId: string;
};
