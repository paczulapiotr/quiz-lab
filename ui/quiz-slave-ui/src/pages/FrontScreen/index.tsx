import styles from "./index.module.scss";
import EraserCanvas from "./EraserCanvas";
import IceImage from "@/assets/images/ice.png";

const FrontScreen = () => {
  return (
    <div className={styles.container}>
      {/* <img src={WelcomeLogo} alt="welcome image" /> */}
      <EraserCanvas
        eraserSize={60}
        animationTime={2000}
        spreadSize={50}
        clearPercentage={90}
        onCleared={() => alert("CLEARED")}
        backgroundImageUrl={IceImage}
      />
    </div>
  );
};

export default FrontScreen;
