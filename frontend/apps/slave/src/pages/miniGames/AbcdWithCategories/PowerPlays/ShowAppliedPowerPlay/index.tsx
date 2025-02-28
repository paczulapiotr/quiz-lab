import Component from "./Component";
import { AbcdDefinition, AbcdState } from "@repo/ui/api/queries/minigames/abcd";
import { useGame } from "@repo/ui/contexts/GameContext";

const ShowAppliedPowerPlay = () => {
  const { players, miniGameState:state, you } = useGame<AbcdState, AbcdDefinition>();

  const powerPlays =
    state?.rounds.find(
      (round) => round.roundId === state?.currentRoundId,
    )?.powerPlays ?? {};
  const applied = powerPlays[you?.id ?? ""] ?? [];
  const appliedPowerPlays = applied.map((pp) => ({
    playerId: pp.fromPlayerId,
    playerName: players.find((p) => p.id === pp.fromPlayerId)?.name ?? "",
    powerPlay: pp.powerPlay,
  }));

  return <Component appliedPowerPlays={appliedPowerPlays} />;
};

export default ShowAppliedPowerPlay;
