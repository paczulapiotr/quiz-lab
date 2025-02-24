import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import "./App.scss";
import GameRoutes from "./Routes";
import FrontScreen from "./pages/FrontScreen";
import { PlayersProvider } from "@repo/ui/contexts/PlayersContext";
import { RoomProvider } from "@repo/ui/contexts/RoomContext";

function App() {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        cacheTime: 0,
        staleTime: Infinity,
        refetchOnMount: "always",
      },
    },
  });
  
  return (
    <QueryClientProvider client={queryClient}>
      <RoomProvider isHost={false}>
          <PlayersProvider>
            <BrowserRouter basename="/slave_ui">
              <Routes>
                <Route index element={<Welcome />} />
                <Route path="front" element={<FrontScreen />} />
                <Route path="*" element={<GameRoutes />} />
              </Routes>
            </BrowserRouter>
          </PlayersProvider>
      </RoomProvider>
    </QueryClientProvider>
  );
}

export default App;
