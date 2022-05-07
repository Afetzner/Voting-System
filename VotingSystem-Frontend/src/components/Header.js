import "./Header.css";
import Logo from "../assets/logo.png";
import { useState } from "react";
import { Link } from "react-router-dom";
import { Button, Container, Modal, Nav, Navbar, NavDropdown } from "react-bootstrap";
import UserAvatar from "./UserAvatar";

function AccountInfo(props) {
  return (
    <>
      <Modal show={props.show} onHide={() => props.setShow(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Account Info</Modal.Title>
        </Modal.Header>
        <Modal.Body>
            {(props.user.isAdmin) && <strong>### ADMIN ACCOUNT ###<br /></strong>}
            <strong>Name: </strong>{props.user.firstName + " " + props.user.lastName}<br />
            <strong>Username: </strong>{props.user.username}<br />
            <strong>Email: </strong>{props.user.email}<br />
            <strong>Serial: </strong>{props.user.serialNumber}<br />
        </Modal.Body>
        <Modal.Footer>
          <Button variant="primary" onClick={() => props.setShow(false)}>Close</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

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
                <NavDropdown.Item onClick={() => props.setShow(true)}>Account Info</NavDropdown.Item>
                <NavDropdown.Divider /></>}
              <NavDropdown.Item>About</NavDropdown.Item>
              <NavDropdown.Item>Enable Dark Mode</NavDropdown.Item>
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
  const [show, setShow] = useState(false);

  return (
    <>
      {(props.user !== null) ? <AccountInfo show={show} setShow={setShow} user={props.user} /> : undefined}
      <Navbar className="nav-bar" bg="primary" variant="dark" >
        <Container>
          <Link to="/">
            <Navbar.Brand placement="start">
              <img alt="" src={Logo} width="64px" height="64px" />
              <div className="brand-text">Voting System</div>
            </Navbar.Brand>
          </Link>
          <UserDropdown setShow={setShow} user={props.user} setUser={props.setUser} setRemember={props.setRemember} />
        </Container>
      </Navbar>
    </>
  );
}