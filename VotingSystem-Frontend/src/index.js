import ReactDOM from "react-dom/client";
import { useEffect, useState } from "react";
import { BrowserRouter, Outlet, Routes, Route } from "react-router-dom";
import "./index.css";
import Header from "./components/Header";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";

function App() {
  const [user, setUser] = useState(JSON.parse(localStorage.getItem("User")));
  const [remember, setRemember] = useState((user !== null));
  const [dark, setDark] = useState(JSON.parse(localStorage.getItem("Dark")));

  useEffect(() => {
    localStorage.setItem("User", (user === null || !remember) ? null : JSON.stringify(user));
  }, [user, remember]);

  useEffect(() => {
    localStorage.setItem("Dark", JSON.stringify(dark));
  }, [dark]);

  return (
    <>
      {(dark)
        ? <link href="https://cdn.jsdelivr.net/npm/bootstrap-dark-5@1.1.3/dist/css/bootstrap-night.min.css" rel="stylesheet" />
        : <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />}
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Layout user={user} setUser={setUser} setRemember={setRemember} dark={dark} setDark={setDark} />}>
            <Route index element={<Vote user={user} setUser={setUser} dark={dark} />} />
            <Route path="sign-in" element={<SignIn user={user} setUser={setUser} remember={remember} setRemember={setRemember} />} />
          </Route>
        </Routes>
      </BrowserRouter>
    </>
  );
}

function Layout(props) {
  return (
    <>
      <Header user={props.user} setUser={props.setUser} setRemember={props.setRemember} dark={props.dark} setDark={props.setDark} />
      <Outlet />
    </>
  );
}

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <App />
);