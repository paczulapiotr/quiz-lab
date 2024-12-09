import "./App.scss";
import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import StateNavigator from "./Navigator";
import { JoinGame } from "./pages/JoinGame";
import { LocalSyncServiceProvider } from "quiz-common-ui";
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
      <LocalSyncServiceProvider
        wsUrl={import.meta.env.VITE_LOCAL_API_URL + "/sync"}
      >
        <BrowserRouter>
          <StateNavigator>
            <Routes>
              <Route index element={<Welcome />} />
              <Route path="/join/:gameId" element={<JoinGame />} />
              <Route path="/starting/:gameId" element={<h1>GameStarting</h1>} />
              <Route path="/rules/:gameId" element={<h1>RulesExplaining</h1>} />
              <Route path="/round/:gameId" element={<h1>RoundStarting</h1>} />
              <Route path="/round/end/:gameId" element={<h1>RoundEnding</h1>} />
              <Route path="/end/:gameId" element={<h1>GameEnding</h1>} />
            </Routes>
          </StateNavigator>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
