import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useState,
} from "react";
import JoinRoom from "./JoinRoom";
import { getDevice } from "../../api/requests/device";

export type Room = {
  code: string;
  uniqueId: string;
  gameId?: string;
};

type RoomContextProps = {
  room?: Room;
};

const RoomContext = createContext<RoomContextProps | undefined>(undefined);

export const RoomProvider: React.FC<{
  children: ReactNode;
  isHost: boolean;
}> = ({ children, isHost }) => {
  const [room, setRoom] = useState<Room>();
  const [loading, setLoading] = useState(true);

  const onJoin = useCallback(
    (roomCode: string, uniqueId: string, gameId?:string) => {
      setRoom({ code: roomCode, uniqueId, gameId });
      sessionStorage.setItem("roomCode", roomCode);
      sessionStorage.setItem(isHost ? "hostId" : "deviceId", uniqueId);
    },
    [isHost]
  );

  useEffect(() => {
    getDevice()
      .then((device) => {
        const uniqueId = isHost ? device?.hostId : device?.deviceId;
        if (device != null && device.roomCode != null && uniqueId != null) {
          onJoin(device.roomCode, uniqueId, device.gameId);
        }
      })
      .finally(() => setLoading(false));
  }, [isHost, onJoin]);

  return (
    <RoomContext.Provider value={{ room }}>
      {loading ? (
        "..."
      ) : room != null ? (
        children
      ) : (
        <JoinRoom isHost={isHost} onJoin={onJoin} />
      )}
    </RoomContext.Provider>
  );
};

export const useRoom = (): RoomContextProps => {
  const context = useContext(RoomContext);
  if (!context) {
    throw new Error("useRoom must be used within a RoomProvider");
  }

  return context;
};
