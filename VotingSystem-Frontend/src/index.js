import React from "react";
import ReactDOM from "react-dom/client";
import { useEffect, useState } from "react";
import { BrowserRouter, Outlet, Routes, Route } from "react-router-dom";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import axios from "axios";
import Header from "./components/Header";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";

function App() {
  const [user, setUser] = useState(undefined);

  useEffect(() => {
    axios.get("https://localhost:7237/api/user").then((response) => {
      console.log(response.data);
      setUser(response.data);
    }).catch(error => {
      console.log(error);
    });
  }, []);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout user={user} setUser={setUser} />}>
          <Route index element={<Vote user={user} setUser={setUser} />} />
          <Route path="sign-in" element={<SignIn user={user} setUser={setUser} />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

function Layout(props) {
  return (
    <>
      <Header user={props.user} setUser={props.setUser} />
      <Outlet />
    </>
  );
}

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);