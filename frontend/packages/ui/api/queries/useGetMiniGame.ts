import { useQuery } from "react-query";
import { DefinitionBase, getMiniGame, StateBase } from "../requests/miniGame";
import QueryKeys from "../QueryKeys";

export const useGetMiniGame = <S extends StateBase, D extends DefinitionBase>(gameId?: string) => {
  return useQuery({
    enabled: Boolean(gameId),
    queryFn: () => getMiniGame<S,D>(gameId!),
    queryKey: [QueryKeys.MINI_GAME, gameId],
  });
};
