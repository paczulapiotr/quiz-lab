import { FlyingSquare } from "quiz-common-ui/components";

const Welcome = () => {
  return (
    <div>
      <h1>Quiz Lab</h1>
      <p>Waiting for game...</p>
      <FlyingSquare count={5} />
    </div>
  );
};

export default Welcome;
