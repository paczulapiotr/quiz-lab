import "./App.scss";
import { LocalSyncServiceProvider } from "./contexts/LocalSyncServiceContext/Provider";
import { JoinGame } from "./pages/JoinGame";

const apiUrl = import.meta.env.VITE_LOCAL_API_URL;

function App() {
  fetch(`${apiUrl}/api/helloworld?who=piotr`)
    .then((res) => res.json())
    .then(console.log);

  return (
    <LocalSyncServiceProvider>
      <JoinGame />
    </LocalSyncServiceProvider>
  );
}

export default App;
