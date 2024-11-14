import { useEffect } from "react";
import "./App.scss";
import * as signalR from "@microsoft/signalr";
import StandardQuestionPage from "./pages/StandardQuestionPage";
import { FlyingSquare } from "./components/FlyingSquare";

const apiUrl = "http://192.168.0.247:5123";

function App() {
  fetch(`${apiUrl}/health`)
    .then((res) => res.json())
    .then(console.log);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(`${apiUrl}/pingpong`)
      .withAutomaticReconnect()
      .build();

    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR hub");

        // Send "Ping" action every 5 seconds
        const intervalId = setInterval(() => {
          connection
            .send("Ping")
            .then(() => console.log("Ping sent"))
            .catch((err) => console.error("Error sending Ping:", err));
        }, 5000);

        // Clean up the interval and connection on component unmount
        return () => {
          console.log("Cleaning up...");
          clearInterval(intervalId);
          connection.stop().then(() => console.log("Connection stopped"));
        };
      })
      .catch((err) => console.error("Error connecting to SignalR hub:", err));
  }, []);

  return (
    <>
      <FlyingSquare count={5} />
      <StandardQuestionPage />
    </>
  );
}

export default App;
