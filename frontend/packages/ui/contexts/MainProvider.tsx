import { PropsWithChildren } from "react";
import { RoomProvider } from "./RoomContext";
import { LocalSyncServiceProvider } from "./LocalSyncServiceContext/Provider";
import { GameProvider } from "./GameContext";

export const MainProvider = ({
  children,
  isHost = false,
}: PropsWithChildren & { isHost: boolean }) => {
  return (
    <RoomProvider isHost={isHost}>
      <LocalSyncServiceProvider>
        <GameProvider>{children}</GameProvider>
      </LocalSyncServiceProvider>
    </RoomProvider>
  );
};
