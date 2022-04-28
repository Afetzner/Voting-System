import { useState } from "react";
import { Outlet } from "react-router-dom";
import Header from "../components/Header";

function User(serialNumber, username, password, email, firstName, lastName, isAdmin) {
  this.serialNumber = serialNumber;
  this.username = username;
  this.password = password;
  this.email = email;
  this.firstName = firstName;
  this.lastName = lastName;
  this.name = firstName + " " + lastName;
  this.isAdmin = isAdmin;
}

export default function Layout() {
  const [user, setUser] = useState(new User("V1234", "abusch8", "1234", "abusch8@huskers.unl.edu", "Alex", "Busch", false));
  return (
    <>
      <Header user={user} setUser={setUser} />
      <Outlet />
    </>
  );
}