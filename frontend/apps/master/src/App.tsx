import "./App.scss";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import GameRoutes from "./Routes";
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
      <MainProvider isHost={true}>
        <BrowserRouter basename="/master_ui">
          <Routes>
            <Route index element={<GameRoutes />} />
          </Routes>
        </BrowserRouter>
      </MainProvider>
    </QueryClientProvider>
  );
}

export default App;
