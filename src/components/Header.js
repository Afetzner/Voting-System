import "./Header.css";
import Logo from "../assets/logo.png";
import { Container, Navbar } from "react-bootstrap";

export default function Header() {
  return (
    <Navbar className="nav-bar" bg="primary" variant="dark">
      <Container>
        <Navbar.Brand>
        <img alt="" src={Logo} width="64px" height="64px"/>
        <div className="brand-text">Voting System</div>
        </Navbar.Brand>
      </Container>
    </Navbar>
  );
}