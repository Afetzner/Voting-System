import { useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header";

export default function Layout() {
<<<<<<< HEAD
  const [user, setUser] = useState({username: "abusch8",
                                    email: "abusch8@huskers.unl.edu",
                                    firstname: "Alex",
                                    lastname: "Busch"});
=======
  const [user, setUser] = useState({username: "abusch8", email: "abusch8@huskers.unl.edu", firstname: "Alex", lastname: "Busch"});
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
  return (
    <>
      <Header user={user} setUser={setUser} />
      <Outlet />
    </>
  );
}