import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useState,
} from "react";
import JoinRoom from "./JoinRoom";

export type Room = {
  code: string;
  uniqueId: string;
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

  const onJoin = useCallback(
    (roomCode: string, uniqueId: string) => {
      setRoom({ code: roomCode, uniqueId });
      sessionStorage.setItem("roomCode", roomCode);
      sessionStorage.setItem(isHost ? "hostId" : "deviceId", uniqueId);
    },
    [isHost]
  );

  return (
    <RoomContext.Provider value={{ room }}>
      {room != null ? children : <JoinRoom isHost={isHost} onJoin={onJoin} />}
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
