import { FlyingSquare, useLocalSyncConsumer } from "quiz-common-ui";
import { useNavigate } from "react-router";

const Welcome = () => {
  const navigate = useNavigate();

  useLocalSyncConsumer("GameCreated", (payload) => {
    console.log(
      `GameCreated, id: ${payload?.gameId}, slots: ${payload?.gameSize}`,
    );
    navigate(`/join/${payload?.gameId}`);
  });

  return (
    <div>
      <h1>Quiz Lab</h1>
      <p>Waiting for game...</p>
      <FlyingSquare count={5} />
    </div>
  );
};

export default Welcome;
