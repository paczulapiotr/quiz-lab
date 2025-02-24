import "./App.scss";
import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import GameRoutes from "./Routes";
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
      <PlayersProvider>
        <RoomProvider isHost={true}>
          <BrowserRouter basename="/master_ui">
            <Routes>
              <Route index element={<Welcome />} />
              <Route path="*" element={<GameRoutes />} />
            </Routes>
          </BrowserRouter>
        </RoomProvider>
      </PlayersProvider>
    </QueryClientProvider>
  );
}

export default App;
