import { useGetMiniGame } from "@repo/ui/api/queries/useGetMiniGame";
import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { usePlayers } from "@repo/ui/contexts/PlayersContext";

type Props = {
  gameId: string;
};

const ShowAppliedPowerPlay = ({ gameId }: Props) => {
  const { data } = useGetMiniGame<AbcdState, AbcdDefinition>(gameId);
  const { players } = usePlayers();

  const powerPlays =
    data?.state?.rounds.find(
      (round) => round.roundId === data?.state?.currentRoundId,
    )?.powerPlays ?? {};
  const playersData = players.map((player) => ({
    playerId: player.id,
    playerName: player.name,
    powerPlays: powerPlays[player.id].map((pp) => ({
      playerId: pp.fromPlayerId,
      playerName: players.find((p) => p.id === pp.fromPlayerId)?.name ?? "",
      powerPlay: pp.powerPlay,
    })),
  }));

  return <Component players={playersData} />;
};

export default ShowAppliedPowerPlay;
