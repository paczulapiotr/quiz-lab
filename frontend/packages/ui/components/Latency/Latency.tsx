import { useCallback, useEffect, useRef, useState } from "react";
import styles from "./Latency.module.scss";
import { useLocalSyncConsumer } from "../../hooks";
import { useLocalSyncService } from "../../hooks/useLocalSyncService";

const Latency = () => {
  const [latency, setLatency] = useState<number>(0);
  const timestamp = useRef<number>(Date.now());

  const { connected, sendSync } = useLocalSyncService();

  useEffect(() => {
    if (!connected) return;

    const interval = setInterval(() => {
      timestamp.current = Date.now();
      // sendSync("Ping");
    }, 5_000);

    return () => clearInterval(interval);
  }, [connected, sendSync]);

  useLocalSyncConsumer(
    "Pong",
    useCallback(async () => {
      const newLatency = Date.now() - timestamp.current;
      setLatency(newLatency);
    }, []),
  );

  return (
    <span className={styles.latency}>{connected ? `${latency}ms` : "---"}</span>
  );
};

export default Latency;
