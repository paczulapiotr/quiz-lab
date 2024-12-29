import classNames from "classnames";
import styles from "./Tile.module.scss";

type Props = {
  text: string;
  blue?: boolean;
  className?: string;
  selected?: boolean;
  success?: boolean;
  failure?: boolean;
  onClick?: () => void;
};

const Tile = ({
  text,
  blue = false,
  selected = false,
  success = false,
  failure = false,
  onClick,
}: Props) => {
  return (
    <div
      onClick={onClick}
      className={classNames(styles.tile, {
        [styles.blue]: blue,
        [styles.selected]: selected,
        [styles.success]: success,
        [styles.failure]: failure,
      })}
    >
      <p>{text}</p>
    </div>
  );
};

export default Tile;
