import { useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header";

function User(username, password, email, firstName, lastName) {
  this.username = username;
  this.password = password;
  this.email = email;
  this.firstName = firstName;
  this.lastName = lastName;
  this.name = firstName + " " + lastName; 
}

export default function Layout() {
  const [user, setUser] = useState(new User("abusch8", "1234", "abusch8@huskers.unl.edu", "Alex", "Busch"));
  return (
    <>
      <Header user={user} setUser={setUser} />
      <Outlet />
    </>
  );
}