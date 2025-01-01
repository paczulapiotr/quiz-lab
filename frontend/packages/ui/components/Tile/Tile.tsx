import classNames from "classnames";
import styles from "./Tile.module.scss";

type Props = {
  text: string;
  blue?: boolean;
  className?: string;
  selected?: boolean;
  success?: boolean;
  failure?: boolean;
};

const Tile = ({
  text,
  blue = false,
  selected = false,
  success = false,
  failure = false,
  className,
}: Props) => {
  return (
    <div
      className={classNames(
        styles.tile,
        {
          [styles.blue]: blue,
          [styles.selected]: selected,
          [styles.success]: success,
          [styles.failure]: failure,
        },
        className
      )}
    >
      <p>{text}</p>
    </div>
  );
};

export default Tile;
