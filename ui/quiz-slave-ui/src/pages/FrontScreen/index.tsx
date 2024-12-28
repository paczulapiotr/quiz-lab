import WelcomeLogo from "@/assets/images/welcome-logo.png";
import styles from "./index.module.scss";
import EraserCanvas from "./EraserCanvas";

const FrontScreen = () => {
  return (
    <div className={styles.container}>
      {/* <img src={WelcomeLogo} alt="welcome image" /> */}
      <EraserCanvas />
    </div>
  );
};

export default FrontScreen;
