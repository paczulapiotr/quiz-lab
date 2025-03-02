import { useEffect, useState } from "react";
import { getDevice, registerDevice } from "../../api/requests/device";
import styles from "./JoinRoom.module.scss";
import { BackgroundLogo } from "../../components/BackgroundLogo";

type Props = {
  onJoin: (roomCode: string, uniqueId: string) => void;
  isHost: boolean;
};

const JoinRoom = ({ onJoin, isHost }: Props) => {
  const [loading, setLoading] = useState(true);
  const [code, setCode] = useState("");

  useEffect(() => {
    getDevice().then((data) => {
      if (data && data.roomCode) {
        onJoin(data.roomCode, isHost ? data.hostId! : data.deviceId!);
      }
      setLoading(false);
    });
  }, [isHost, onJoin]);

  const handleJoin = async () => {
    const result = await registerDevice(code, isHost);

    if (!result.ok) {
      alert("Error joining room");
      return;
    }

    onJoin(code, result.uniqueId!);
  };

  return (
    <>
      <BackgroundLogo />
      <section className={styles.join}>
        {loading ? (
          <h1 className={styles.loading}>{"Loading"}</h1>
        ) : (
          <div className={styles.control}>
            <input value={code} onChange={(e) => setCode(e.target.value)} />
            <button onClick={handleJoin}>
              {isHost ? "Create Room" : "Join Room"}
            </button>
          </div>
        )}
      </section>
    </>
  );
};

export default JoinRoom;
