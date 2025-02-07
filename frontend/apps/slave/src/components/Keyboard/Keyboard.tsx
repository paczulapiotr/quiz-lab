import SimpleKeyboard from "react-simple-keyboard";
import styles from "./Keyboard.module.scss";
import "react-simple-keyboard/build/css/index.css";

type Props = {
  onKeyPress?: (key: string) => void;
  onChange?: (input: string) => void;
};

const Keyboard = ({ onChange, onKeyPress }: Props) => {
  return (
    <SimpleKeyboard
      layoutName={"default"}
      onChange={onChange}
      onKeyPress={onKeyPress}
      theme={`hg-theme-default ${styles.theme}`}
    />
  );
};

export default Keyboard;
