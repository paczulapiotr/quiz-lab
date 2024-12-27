import { Tile } from "../Tile";
import styles from "./HeaderTile.module.scss";

type Props = {
  title: string;
};

const HeaderTile = ({ title }: Props) => {
  return <Tile text={title} blue className={styles.questionTile} />;
};

export default HeaderTile;
