// import { GameStatus } from "quiz-common-ui";
// import { useLocalSync } from "quiz-common-ui/hooks";
// import { useEffect } from "react";
// import { useParams } from "react-router";

const GameStarting = () => {
  //   const { sendSync } = useLocalSync();
  //   const { gameId } = useParams<{ gameId: string }>();

  //   useEffect(() => {
  //     const timer = setTimeout(() => {
  //       sendSync("GameStatusUpdate", {
  //         gameId: gameId!,
  //         status: GameStatus.GameStarted,
  //       });
  //     }, 10_000);

  //     return () => clearTimeout(timer);
  //   }, [gameId, sendSync]);

  return (
    <>
      <h1>GameStarting</h1>
    </>
  );
};

export default GameStarting;
