import "./App.scss";
import StandardQuestionPage from "./pages/StandardQuestionPage";
import { JoinGame } from "./pages/JoinGame";
import { Welcome } from "./pages/Welcome";
import { BrowserRouter, Route, Routes } from "react-router";
import { QueryClient, QueryClientProvider } from "react-query";
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
          <Routes>
            <Route index element={<Welcome />} />
            <Route path="join/:gameId" element={<JoinGame />} />
            <Route path=":gameId/question" element={<StandardQuestionPage />} />
          </Routes>
        </BrowserRouter>
      </LocalSyncServiceProvider>
    </QueryClientProvider>
  );
}

export default App;
