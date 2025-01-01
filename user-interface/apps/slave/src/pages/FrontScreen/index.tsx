import styles from "./index.module.scss";
import WelcomeLogo from "@/assets/images/welcome-logo.png";

const FrontScreen = () => {
  return (
    <div className={styles.container}>
      <img src={WelcomeLogo} alt="welcome image" />
    </div>
  );
};

export default FrontScreen;
