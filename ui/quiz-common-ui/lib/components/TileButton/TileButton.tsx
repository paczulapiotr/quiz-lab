import classNames from "classnames";
import styles from "./TileButton.module.scss";

type Props = {
  text: string;
  blue?: boolean;
  className?: string;
  selected?: boolean;
  success?: boolean;
  failure?: boolean;
  onClick?: () => void;
};

const TileButton = ({
  text,
  blue = false,
  selected = false,
  success = false,
  failure = false,
  className,
  onClick,
}: Props) => {
  return (
    <button
      onClick={onClick}
      className={classNames(
        styles.tileButton,
        {
          [styles.blue]: blue,
          [styles.selected]: selected,
          [styles.success]: success,
          [styles.failure]: failure,
        },
        className,
      )}
    >
      <span>{text}</span>
    </button>
  );
};

export default TileButton;
