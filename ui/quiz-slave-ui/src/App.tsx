import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import { LocalSyncServiceProvider } from "quiz-common-ui";
import "./App.scss";
import GameRoutes from "./Routes";

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
            <Route path="*" element={<GameRoutes />} />
          </Routes>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
