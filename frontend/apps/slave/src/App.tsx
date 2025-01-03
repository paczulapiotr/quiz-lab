import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import "./App.scss";
import GameRoutes from "./Routes";
import FrontScreen from "./pages/FrontScreen";
import { LocalSyncServiceProvider } from "@repo/ui/contexts";

function App() {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        staleTime: 0,
        cacheTime: 0,
      },
    },
  });

  return (
    <QueryClientProvider client={queryClient}>
      <LocalSyncServiceProvider
        wsUrl={import.meta.env.VITE_LOCAL_API_URL + "/sync"}
      >
        <BrowserRouter>
          <Routes>
            <Route index element={<Welcome />} />
            <Route path="front" element={<FrontScreen />} />
            <Route path="*" element={<GameRoutes />} />
          </Routes>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
