import { memo, useEffect, useMemo, useRef, useState } from "react";
import SimpleKeyboard from "react-simple-keyboard";
import { KeyboardOptions, mapLayout, SpecialButtonKeys } from "./layout";
import styles from "./Keyboard.module.scss";
import "react-simple-keyboard/build/css/index.css";

type Props = {
  value?: string;
  onKeyPress?: (key: string) => void;
  onChange?: (input: string) => void;
  disabledLetters?: string[];
  defaultUpperCase?: boolean;
} & KeyboardOptions;

const Keyboard = ({
  value,
  onChange,
  onKeyPress,
  defaultUpperCase,
  disabledLetters,
  ...keyboardOpts
}: Props) => {
  const ref = useRef<{ setInput: (v: string) => void, input: { default: string } }>();
  const [layout, setLayout] = useState(defaultUpperCase ? "shift" : "default");

  useEffect(() => {
    if(ref.current?.input.default !== value ) {
      ref.current?.setInput(value ?? "");
    }
  },[value])


  const onChangeHandler = (input: string) => {
    onChange && onChange(input);
  };

  const onKeyPressHandler = (key: string) => {
    if (!SpecialButtonKeys.includes(key)) {
      onKeyPress && onKeyPress(key);
    } else if (key === "{shift}") {
      setLayout(layout === "default" ? "shift" : "default");
    }
  };
  
  const btnAttributes = useMemo(() => 
    disabledLetters?.map((l) => ({
      buttons: `${l} ${l.toUpperCase()}`,
      attribute: "disabled",
      value: "true",
    })) ?? [], [disabledLetters]
  )

  return (
    <SimpleKeyboard
      keyboardRef={(r) => (ref.current = r)}
      useButtonTag
      display={{ "{shift}": "⇧", "{bksp}": "⌫", "{space}": " " }}
      layoutName={layout}
      buttonAttributes={btnAttributes}
      layout={mapLayout(keyboardOpts)}
      onChange={onChangeHandler}
      onKeyPress={onKeyPressHandler}
      theme={`hg-theme-default ${styles.theme}`}
    />
  );
};

export default memo(Keyboard);
