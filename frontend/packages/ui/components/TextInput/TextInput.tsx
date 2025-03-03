import { useCallback, useState } from "react";
import styles from "./TextInput.module.scss";
import { TileButton } from "../TileButton";
import classNames from "classnames";

type Props = {
  onClick: (text: string) => void;
  buttonText: string;
  placeholder?: string;
  small?: boolean;
  value?: string;
  onChange?: (value: string) => void;
};

const TextInput = ({
  onClick,
  buttonText,
  placeholder,
  small,
  onChange,
  value: _value,
}: Props) => {
  const [value, setValue] = useState("");
  const v = _value ?? value;

  const handleClick = useCallback(() => {
    onClick(v);
  }, [onClick, v]);

  return (
    <div className={classNames(styles.input, { [styles.small]: small })}>
      <input
        maxLength={40}
        className={styles.text}
        type="text"
        placeholder={placeholder}
        value={v}
        onChange={(e) => {
          setValue(e.target.value);
          onChange?.(e.target.value);
        }}
      />
      <TileButton
        text={buttonText}
        blue
        onClick={handleClick}
        className={styles.button}
      />
    </div>
  );
};

export default TextInput;
