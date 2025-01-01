import SampleMp4 from "@/assets/videos/sample_720.mp4";
import styles from "./TutorialVideo.module.scss";

type Props = {
  title: string;
};

const TutorialVideo = ({ title }: Props) => {
  return (
    <div className={styles.tutorial}>
      <video src={SampleMp4} autoPlay width={1920} height={1080} />
      <p
        style={{
          position: "fixed",
          display: "block",
          top: "2rem",
          left: 0,
          width: "100%",
          zIndex: 1,
          padding: "1rem",
          textAlign: "center",
          color: "white",
          background: "rgba(60, 60, 60, 0.5)",
        }}
      >
        {title}
      </p>
    </div>
  );
};

export default TutorialVideo;
