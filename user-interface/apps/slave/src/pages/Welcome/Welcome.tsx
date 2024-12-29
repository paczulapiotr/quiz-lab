import { GameStatus } from "@repo/ui";
import { FlyingSquare } from "@repo/ui/components";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { useCallback } from "react";
import { useNavigate } from "react-router";

const Welcome = () => {
  const navigate = useNavigate();

  useLocalSyncConsumer(
    "GameStatusUpdate",
    "Welcome",
    useCallback(
      (payload) => {
        if (payload?.status === GameStatus.GameCreated) {
          navigate(`/game/${payload?.gameId}/join`);
        }
      },
      [navigate],
    ),
  );

  return (
    <div>
      <h1>Quiz Lab</h1>
      <p>Waiting for game...</p>
      <FlyingSquare count={5} />
    </div>
  );
};

export default Welcome;
