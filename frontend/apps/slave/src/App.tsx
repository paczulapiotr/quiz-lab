import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import "./App.scss";
import GameRoutes from "./Routes";
import FrontScreen from "./pages/FrontScreen";
import { MainProvider } from "@repo/ui/contexts/MainProvider";

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
      <MainProvider isHost={false}>
        <BrowserRouter basename="/slave_ui">
          <Routes>
            <Route index element={<GameRoutes />} />
            <Route path="front" element={<FrontScreen />} />
          </Routes>
        </BrowserRouter>
      </MainProvider>
    </QueryClientProvider>
  );
}

export default App;
