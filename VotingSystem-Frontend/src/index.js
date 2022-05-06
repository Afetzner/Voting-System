import React from "react";
import ReactDOM from "react-dom/client";
import { useEffect, useState } from "react";
import { BrowserRouter, Outlet, Routes, Route } from "react-router-dom";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import Header from "./components/Header";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";

function App() {
  const [user, setUser] = useState(JSON.parse(localStorage.getItem("User")));
  const [remember, setRemember] = useState((user !== null));

  useEffect(() => {
    localStorage.setItem("User", (user === null || !remember) ? null : JSON.stringify(user));
  }, [user, remember]);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout user={user} setUser={setUser} setRemember={setRemember} />}>
          <Route index element={<Vote user={user} setUser={setUser} />} />
          <Route path="sign-in" element={<SignIn user={user} setUser={setUser} remember={remember} setRemember={setRemember} />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

function Layout(props) {
  return (
    <>
      <Header user={props.user} setUser={props.setUser} setRemember={props.setRemember} />
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