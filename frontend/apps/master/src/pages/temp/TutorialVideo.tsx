import SampleMp4 from "@/assets/videos/sample_720.mp4";
import styles from "./TutorialVideo.module.scss";

type Props = {
  url?: string;
};

const TutorialVideo = ({ url }: Props) => {
  return <video className={styles.tutorial} src={url ?? SampleMp4} autoPlay width={1920} height={1080} />;
};

export default TutorialVideo;
