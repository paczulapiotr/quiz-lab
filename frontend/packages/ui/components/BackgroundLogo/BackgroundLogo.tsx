import WelcomeLogo from "@repo/ui/assets/images/welcome-logo.png";
import styles from "./BackgroundLogo.module.scss";

const BackgroundLogo = () => {
  return (
    <div className={styles.container}>
      <img src={WelcomeLogo} alt="welcome image" />
    </div>
  );
};

export default BackgroundLogo;
