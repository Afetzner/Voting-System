import { useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header";

export default function Layout() {
  const [user, setUser] = useState({username: "abusch8", email: "abusch8@huskers.unl.edu", firstname: "Alex", lastname: "Busch"});
  return (
    <>
      <Header user={user} setUser={setUser} />
      <Outlet />
    </>
  );
}