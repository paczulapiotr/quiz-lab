import WelcomeLogo from "@/assets/images/welcome-logo.png";
import styles from "./Welcome.module.scss";
import { useCallback } from "react";
import { useNavigate } from "react-router";
import { useLocalSyncConsumer } from "@repo/ui/hooks";
import { GameStatus } from "@repo/ui/services/types";

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
