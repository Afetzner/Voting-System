import React from "react";
import ReactDOM from "react-dom/client";
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
import { BrowserRouter, Routes, Route } from "react-router-dom";
import "./index.css";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";
<<<<<<< HEAD
import Layout from "./views/Layout";
=======
import "./index.css";
import SignIn from "./views/SignIn";
import Vote from "./views/Vote";
>>>>>>> ae1b665 (Initial commit)
=======
import Layout from "./views/Layout"
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
import "bootstrap/dist/css/bootstrap.min.css";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Vote />} />
<<<<<<< HEAD
          <Route path="signin" element={<SignIn />} />
        </Route>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);
=======
    <Vote />
=======
          <Route path="sign-in" element={<SignIn />} />
        </Route>
      </Routes>
    </BrowserRouter>
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
  </React.StrictMode>
<<<<<<< HEAD
);
>>>>>>> ae1b665 (Initial commit)
=======
);
>>>>>>> 872a854 (Converted Confirm.js to a more general PopUp.js, syntax clean up)
