import "./Header.css";
import Logo from "../assets/logo.png";
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
  if (props.user === undefined) {
    return (
      <Avatar />
    );
  } else {
    return (
      <Avatar sx={{bgcolor: color(props.user.firstName + " " + props.user.lastName)}}>
        {`${props.user.firstName[0]}${props.user.lastName[0]}`}
      </Avatar>
    );
  }
}

function UserDropdown(props) {
  return (
    <div className="user">
      <Nav>
        <Container>
          {(props.user === undefined) ? 
          <Link to="/sign-in">
            <Button variant="outline-light">Sign In</Button> 
          </Link>
          : undefined}
        </Container>
        <UserAvatar user={props.user} />
        <Navbar.Collapse>
          <Nav>
            <NavDropdown align="end" title={(props.user === undefined) ? "" : props.user.username}>
              <NavDropdown.Item>Account Info</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item>About</NavDropdown.Item>
              <NavDropdown.Item>Enable Dark Mode</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item onClick={() => props.setUser(undefined)}>Sign Out</NavDropdown.Item>
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
      </Container>
    </Navbar>
  );
}