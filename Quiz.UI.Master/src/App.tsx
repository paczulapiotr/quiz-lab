import "./App.scss";
import { LocalSyncServiceProvider } from "./contexts/LocalSyncServiceContext/Provider";
import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import StateNavigator from "./Navigator";
function App() {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: Infinity,
        cacheTime: 0,
      },
    },
  });

  return (
    <QueryClientProvider client={queryClient}>
      <LocalSyncServiceProvider>
        <BrowserRouter>
          <StateNavigator>
            <Routes>
              <Route index element={<Welcome />} />
            </Routes>
          </StateNavigator>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
