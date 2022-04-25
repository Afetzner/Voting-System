import React from "react";
import ReactDOM from "react-dom/client";
<<<<<<< HEAD
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./index.css";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";
import Layout from "./views/Layout";
=======
import "./index.css";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";
>>>>>>> ae1b665 (Initial commit)
import "bootstrap/dist/css/bootstrap.min.css";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
<<<<<<< HEAD
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Vote />} />
          <Route path="signin" element={<SignIn />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
=======
    <Vote />
  </React.StrictMode>
);
>>>>>>> ae1b665 (Initial commit)
