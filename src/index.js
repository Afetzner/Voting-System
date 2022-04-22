import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";
import "bootstrap/dist/css/bootstrap.min.css";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <Vote />
  </React.StrictMode>
);