import styles from "./index.module.scss";
import EraserCanvas from "./EraserCanvas";

const FrontScreen = () => {
  return (
    <div className={styles.container}>
      {/* <img src={WelcomeLogo} alt="welcome image" /> */}
      <EraserCanvas eraserSize={60} onCleared={() => alert("CLEARED")} />
    </div>
  );
};

export default FrontScreen;
