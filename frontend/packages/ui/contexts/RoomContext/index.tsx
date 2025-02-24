import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useState,
} from "react";
import { LocalSyncServiceProvider } from "../LocalSyncServiceContext/Provider";
import JoinRoom from "./JoinRoom";

export type Room = {
  code: string;
  uniqueId: string;
};

type RoomContextProps = {
  room?: Room;
  setRoom: (room?: Room) => void;
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
    <RoomContext.Provider value={{ room, setRoom }}>
      {room != null ? (
        <LocalSyncServiceProvider
          listenToMessages={["GameStatusUpdate", "MiniGameNotification"]}
          wsUrl={
            import.meta.env.VITE_LOCAL_API_URL +
            `/sync?uniqueId=${room.uniqueId}`
          }
        >
          {children}
        </LocalSyncServiceProvider>
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
