import "./Header.css";
import Logo from "../assets/logo.png";
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
import { Link } from "react-router-dom";
import { Button, Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Avatar } from "@mui/material";

const color = (name) => {
  let hash = 0, color = "#";
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash);
  }
  for (let i = 0; i < 3; i++) {
    const value = (hash >> (i * 8)) & 0xff;
    color += `00${value.toString(16)}`.slice(-2);
  }
  return color;
};

function UserAvatar(props) {
  if (props.user === null) {
    return (
      <Avatar />
    );
  } else {
    return (
      <Avatar sx={{bgcolor: color(props.user.firstname + " " + props.user.lastname)}}>
        {`${props.user.firstname[0]}${props.user.lastname[0]}`}
      </Avatar>
    );
  }
}

function UserDropdown(props) {
  return (
    <div className="user">
      <Nav>
        <Container>
          {(props.user === null) ? 
          <Link to="/signin">
            <Button variant="outline-light">Sign In</Button> 
          </Link>
          : undefined}
        </Container>
        <UserAvatar user={props.user} />
        <Navbar.Collapse>
          <Nav>
            <NavDropdown align="end" title={(props.user === null) ? "" : props.user.username}>
              <NavDropdown.Item>Account Info</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item>About</NavDropdown.Item>
              <NavDropdown.Item>Enable Dark Mode</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item onClick={() => props.setUser(null)}>Sign Out</NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Nav>
    </div>
  );
}

export default function Header(props) {
  return (
    <Navbar className="nav-bar" bg="primary" variant="dark" >
      <Container>
        <Link to="/">
          <Navbar.Brand placement="start">
            <img alt="" src={Logo} width="64px" height="64px" />
            <div className="brand-text">Voting System</div>
          </Navbar.Brand>
        </Link>
        <UserDropdown user={props.user} setUser={props.setUser} />
=======
import { Container, Navbar } from "react-bootstrap";
=======
import { Button, Container, Navbar } from "react-bootstrap";
>>>>>>> 10e0c32 (Continued work on Vote view)
=======
import { Link } from "react-router-dom";
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Avatar } from "@mui/material";
>>>>>>> a86a343 (Implemented router dom and user drop down menu)

function stringToColor(string) {
  let hash = 0;

  /* eslint-disable no-bitwise */
  for (let i = 0; i < string.length; i++) {
    hash = string.charCodeAt(i) + ((hash << 5) - hash);
  }

  let color = '#';

  for (let i = 0; i < 3; i++) {
    const value = (hash >> (i * 8)) & 0xff;
    color += `00${value.toString(16)}`.slice(-2);
  }
  /* eslint-enable no-bitwise */

  return color;
}

function UserDropdown(props) {
  return (
<<<<<<< HEAD
    <Navbar className="nav-bar" bg="primary" variant="dark">
      <Container>
        <Navbar.Brand>
          <img alt="" src={Logo} width="64px" height="64px"/>
          <div className="brand-text">Voting System</div>
        </Navbar.Brand>
<<<<<<< HEAD
>>>>>>> ae1b665 (Initial commit)
=======
        <Button variant="danger">Sign Out</Button>
>>>>>>> 10e0c32 (Continued work on Vote view)
=======
    <div className="user">
      <Avatar 
        sx={{bgcolor: stringToColor(props.user.firstname + " " + props.user.lastname)}}
      >{`${props.user.firstname[0]}${props.user.lastname[0]}`}</Avatar>
      <Navbar.Collapse>
        <Nav>
          <NavDropdown align="end" title={props.user.username}>
            <NavDropdown.Item>Account Info</NavDropdown.Item>
            <NavDropdown.Divider />
            <NavDropdown.Item>Sign Out</NavDropdown.Item>
          </NavDropdown>
        </Nav>
      </Navbar.Collapse>
    </div>
  );
}

export default function Header(props) {
  return (
    <Navbar className="nav-bar" bg="primary" variant="dark" >
      <Container >
        <Link to="/">
          <Navbar.Brand>
            <img alt="" src={Logo} width="64px" height="64px" />
            <div className="brand-text">Voting System</div>
          </Navbar.Brand>
        </Link>
        <UserDropdown user={props.user} setUser={props.setUser} />
>>>>>>> a86a343 (Implemented router dom and user drop down menu)
      </Container>
    </Navbar>
  );
}