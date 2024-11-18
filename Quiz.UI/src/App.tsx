import "./App.scss";
import StandardQuestionPage from "./pages/StandardQuestionPage";
import { FlyingSquare } from "./components/FlyingSquare";
import { LocalSyncServiceProvider } from "./contexts/LocalSyncServiceContext/Provider";

const apiUrl = "http://192.168.0.247:5123";
// const apiUrl = "http://localhost:5123";

function App() {
  fetch(`${apiUrl}/ping`, {
    method: "POST",
    body: JSON.stringify({ Message: "Hello" }),
    headers: { "Content-Type": "application/json" },
  })
    .then((res) => res.json())
    .then(console.log);

  return (
    <LocalSyncServiceProvider>
      <FlyingSquare count={5} />
      <StandardQuestionPage />
    </LocalSyncServiceProvider>
  );
}

export default App;
