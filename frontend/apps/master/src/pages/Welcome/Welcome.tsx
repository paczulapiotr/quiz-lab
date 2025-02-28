import WelcomeLogo from "@/assets/images/welcome-logo.png";
import styles from "./Welcome.module.scss";

const Welcome = () => {
  return (
    <div className={styles.container}>
      <img src={WelcomeLogo} alt="welcome image" />
    </div>
  );
};

export default Welcome;
