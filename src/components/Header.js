import "./Header.css";
import Logo from "../assets/logo.png";
import { Link } from "react-router-dom";
import { Container, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Avatar } from "@mui/material";

function stringToColor(string) {
  /* eslint-disable no-bitwise */
  let hash = 0;
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
    <div className="user">
      <Nav>
        <Avatar sx={{bgcolor: stringToColor(props.user.firstname + " " + props.user.lastname)}}>
          {`${props.user.firstname[0]}${props.user.lastname[0]}`}
        </Avatar>
        <Navbar.Collapse>
          <Nav>
            <NavDropdown align="end" title={props.user.username}>
              <NavDropdown.Item>Account Info</NavDropdown.Item>
              <NavDropdown.Divider />
              <NavDropdown.Item>Sign Out</NavDropdown.Item>
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