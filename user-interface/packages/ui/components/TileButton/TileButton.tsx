import classNames from "classnames";
import styles from "./TileButton.module.scss";
import { useEffect, useLayoutEffect, useRef, useState } from "react";
import { EraserCanvas } from "../EraserCanvas";

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
  slimeImageUrl?: string;
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
  slimeStacks = 0,
  slimeImageUrl,
}: Props) => {
  const buttonRef = useRef<HTMLButtonElement>(null);
  const [slimeWiped, setSlimeWiped] = useState(slimeStacks <= 0);
  const [freezeClick, setFreezeClick] = useState(
    calcMaxFreezeClicks(freezeStacks)
  );

  useEffect(() => {
    if (freezeStacks != null && freezeStacks > 0) {
      setFreezeClick(calcMaxFreezeClicks(freezeStacks));
    }
  }, [freezeStacks]);

  const [slimeWidth, setSlimeWidth] = useState(0);
  const [slimeHeight, setSlimeHeight] = useState(0);

  useLayoutEffect(() => {
    if (buttonRef.current) {
      const { offsetWidth, offsetHeight } = buttonRef.current;
      setSlimeWidth(offsetWidth);
      setSlimeHeight(offsetHeight);
    }
  }, []);

  return (
    <div className={styles.tileWrapper}>
      <button
        ref={buttonRef}
        onClick={onClick}
        className={classNames(
          styles.tileButton,
          {
            [styles.blue]: blue,
            [styles.selected]: selected,
            [styles.success]: success,
            [styles.failure]: failure,
          },
          className
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
      {!slimeWiped ? (
        <EraserCanvas
          spreadSize={100}
          animationTime={10_000}
          canvasWidth={slimeWidth}
          canvasHeight={slimeHeight}
          className={styles.slime}
          backgroundImageUrl={slimeImageUrl}
          clearPercentage={95}
          eraserSize={30 / slimeStacks}
          onCleared={() => setSlimeWiped(true)}
        />
      ) : null}
    </div>
  );
};

export default TileButton;
