import classNames from "classnames";
import styles from "./TileButton.module.scss";
import { useEffect, useState } from "react";

type Props = {
  text: string;
  blue?: boolean;
  className?: string;
  selected?: boolean;
  success?: boolean;
  failure?: boolean;
  onClick?: () => void;
  freezeStacks?: number;
  slimeStacks?: number;
};

const calcMaxFreezeClicks = (freezeStacks?: number) =>
  freezeStacks ? freezeStacks * 3 : 0;

const TileButton = ({
  text,
  blue = false,
  selected = false,
  success = false,
  failure = false,
  className,
  onClick,
  freezeStacks,
}: Props) => {
  const [freezeClick, setFreezeClick] = useState(
    calcMaxFreezeClicks(freezeStacks),
  );

  useEffect(() => {
    if (freezeStacks != null && freezeStacks > 0) {
      setFreezeClick(calcMaxFreezeClicks(freezeStacks));
    }
  }, [freezeStacks]);

  return (
    <div className={styles.tileWrapper}>
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
      {freezeClick > 0 ? (
        <div
          className={styles.freeze}
          style={{ opacity: freezeClick / calcMaxFreezeClicks(freezeStacks) }}
          onClick={() => setFreezeClick((prev) => prev - 1)}
        />
      ) : null}
    </div>
  );
};

export default TileButton;
