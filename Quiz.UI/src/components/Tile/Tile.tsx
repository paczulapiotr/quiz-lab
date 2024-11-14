import classNames from "classnames";
import styles from "./Tile.module.scss";

type Props = { text: string; blue?: boolean; className?: string };

const Tile = ({ text, blue = false }: Props) => {
  return (
    <div className={classNames(styles.tile, { [styles.blue]: blue })}>
      <p>{text}</p>
    </div>
  );
};

export default Tile;
