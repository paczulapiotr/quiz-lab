import styles from "./Welcome.module.scss";
import CreateGame from "./CreateGame";
import { BackgroundLogo } from "@repo/ui/components/BackgroundLogo";

const Welcome = () => {
  return (
    <>
      <BackgroundLogo />
      <CreateGame className={styles.create} />
    </>
  );
};

export default Welcome;
