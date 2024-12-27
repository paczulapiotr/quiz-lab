import { GameStatus } from "quiz-common-ui";
// import { FlyingSquare } from "quiz-common-ui/components";
import { useLocalSyncConsumer } from "quiz-common-ui/hooks";
import { useCallback } from "react";
import { useNavigate } from "react-router";
import WelcomeLogo from "@/assets/images/welcome-logo.png";
import styles from "./Welcome.module.scss";

const Welcome = () => {
  const navigate = useNavigate();

  useLocalSyncConsumer(
    "GameStatusUpdate",
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
    <div className={styles.container}>
      <img src={WelcomeLogo} alt="welcome image" />
    </div>
  );
};

export default Welcome;
