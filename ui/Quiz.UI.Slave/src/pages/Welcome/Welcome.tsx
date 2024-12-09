import { GameStatus } from "quiz-common-ui";
import { FlyingSquare } from "quiz-common-ui/components";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
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
          navigate(`/join/${payload?.gameId}`);
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
