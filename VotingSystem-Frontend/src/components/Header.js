import "./Header.css";
import Logo from "../assets/logo.png";
import { useState } from "react";
import { Link } from "react-router-dom";
import { Button, Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import UserAvatar from "./UserAvatar";
import PopUp from "./PopUp";

function UserDropdown(props) {
  const handleClick = () => {
    props.setUser(null);
    props.setRemember(false);
  };

  return (
    <div className="user">
      <Nav>
        <Container>
          {(props.user === null) && <Button variant="outline-light" as={Link} to={"/sign-in"}>Sign In</Button>}
        </Container>
        <UserAvatar user={props.user} />
        <Navbar.Collapse>
          <Nav>
            <NavDropdown align="end" title={(props.user === null) ? "" : props.user.username}>
              {(props.user !== null) && <>
                <NavDropdown.Item onClick={() => props.setShowInfo(true)}>Account Info</NavDropdown.Item>
                <NavDropdown.Divider /></>}
              <NavDropdown.Item onClick={() => props.setShowAbout(true)}>About</NavDropdown.Item>
              <NavDropdown.Item onClick={() => props.setDark(!props.dark)}>Toggle Dark Mode</NavDropdown.Item>
              <NavDropdown.Divider />
              {(props.user !== null)
                ? <NavDropdown.Item onClick={() => handleClick()}>Sign Out</NavDropdown.Item>
                : <NavDropdown.Item as={Link} to="/sign-in">Sign In</NavDropdown.Item>}
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Nav>
    </div>
  );
}

export default function Header(props) {
  const [showInfo, setShowInfo] = useState(false);
  const [showAbout, setShowAbout] = useState(false);

  return (
    <>
      {(props.user !== null) &&
        <PopUp show={showInfo} setShow={setShowInfo} title={"AccountInfo"}>
          {(props.user.isAdmin) && <strong>### ADMIN ACCOUNT ###<br /></strong>}
          <strong>Name: </strong>{props.user.firstName + " " + props.user.lastName}<br />
          <strong>Username: </strong>{props.user.username}<br />
          <strong>Email: </strong>{props.user.email}<br />
          <strong>Serial: </strong>{props.user.serialNumber}<br />
        </PopUp>}
      <PopUp show={showAbout} setShow={setShowAbout} title={"About"}>
        <strong>Credits:<br /></strong>
        <ul>
          <li><a href="https://github.com/abusch8">abusch8</a></li>
          <li><a href="https://github.com/Afetzner">Afetzner</a></li>
          <li><a href="https://github.com/akrestovsky">akrestovsky</a></li>
          <li><a href="https://github.com/janice616">janice616</a></li>
          <li><a href="https://github.com/Khondamir1">Khondamir1</a></li>
        </ul>
        <a href="https://github.com/Afetzner/CSCE361_voting_system_group_3/tree/develop">GitHub</a><br />
      </PopUp>
      <Navbar className="nav-bar" bg="primary" variant="dark" style={(props.dark) ? {color: "red"} : {}} >
        <Container>
          <Link to="/">
            <Navbar.Brand placement="start">
              <img alt="" src={Logo} width="64px" height="64px" />
              <div className="brand-text">Voting System</div>
            </Navbar.Brand>
          </Link>
          <UserDropdown setShowInfo={setShowInfo} setShowAbout={setShowAbout} user={props.user} setUser={props.setUser} setRemember={props.setRemember} dark={props.dark} setDark={props.setDark} />
        </Container>
      </Navbar>
    </>
  );
}