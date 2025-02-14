import styles from "./Component.module.scss";

type Props = {
  gameId?: string;
};

const Component = ({ gameId }: Props) => {
  return <div className={styles.page}>{gameId}</div>;
};

export default Component;
