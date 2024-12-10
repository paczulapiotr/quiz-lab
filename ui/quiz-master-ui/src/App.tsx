import "./App.scss";
import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
import StateNavigator from "./Navigator";
import { JoinGame } from "./pages/JoinGame";
import { LocalSyncServiceProvider } from "quiz-common-ui";
import { GameStarting } from "./pages/GameStarting";
import { RulesExplaining } from "./pages/RulesExplaining";
import { MiniGameStarting } from "./pages/MiniGameStarting";
import MiniGameEnding from "./pages/MiniGameEnding/MiniGameEnding";
import { GameEnding } from "./pages/GameEnding";
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
              <Route path="/starting/:gameId" element={<GameStarting />} />
              <Route path="/rules/:gameId" element={<RulesExplaining />} />
              <Route path="/minigame/:gameId" element={<MiniGameStarting />} />
              <Route
                path="/minigame/end/:gameId"
                element={<MiniGameEnding />}
              />
              <Route path="/end/:gameId" element={<GameEnding />} />
            </Routes>
          </StateNavigator>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
