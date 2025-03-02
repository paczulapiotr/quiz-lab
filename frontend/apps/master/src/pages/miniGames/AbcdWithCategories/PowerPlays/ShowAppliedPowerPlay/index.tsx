import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowAppliedPowerPlay = () => {
  const { players, miniGameState: state } = useGame<AbcdState, AbcdDefinition>();

  const powerPlays =
    state?.rounds.find(
      (round) => round.roundId === state?.currentRoundId,
    )?.powerPlays ?? {};

  const playersData = players.map((player) => ({
    playerId: player.id,
    playerName: player.name,
    powerPlays: powerPlays[player.id]?.map((pp) => ({
      playerId: pp.fromPlayerId,
      playerName: players.find((p) => p.id === pp.fromPlayerId)?.name ?? "",
      powerPlay: pp.powerPlay,
    })) ?? [],
  }));

  return <Component players={playersData} />;
};

export default ShowAppliedPowerPlay;
