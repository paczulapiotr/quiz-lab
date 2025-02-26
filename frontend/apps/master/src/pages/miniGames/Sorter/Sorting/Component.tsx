
import { useGetMiniGame } from '@repo/ui/api/queries/useGetMiniGame';
import {
  SorterState,
  SorterDefinition,
} from "@repo/ui/api/queries/minigames/sorter";
import { Tile } from '@repo/ui/components';

type Props = {
  gameId?: string;
  started?: boolean;
};

const Component = ({gameId}: Props) => {
  const { data } = useGetMiniGame<SorterState, SorterDefinition>(gameId);
   const roundDef = data?.definition?.rounds.find(
     (x) => x.id == data?.state?.currentRoundId,
   );
  const left = roundDef?.leftCategory.name;
  const right = roundDef?.rightCategory.name;
  
  return <Tile blue text={`Posortuj, ${left} czy ${right}?`} />;
}

export default Component