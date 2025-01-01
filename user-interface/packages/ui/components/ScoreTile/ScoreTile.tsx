import StartSvg from "#/assets/icons/star.svg";
import styles from "./ScoreTile.module.scss";

type Props = {
  score?: number;
};

const ScoreTile = ({ score = 0 }: Props) => {
  return (
    <div className={styles.score}>
      <span>{`${score}`}</span>
      <img src={StartSvg} alt="start icon" />
    </div>
  );
};

export default ScoreTile;
