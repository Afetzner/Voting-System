import "./SignIn.css";
import { useState } from "react";
import { Alert, Button, Card, Form, Spinner } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import axios from "axios";

function Failed(props) {
  if (props.show) {
    return (
      <Alert variant="danger" onClose={() => props.setShow(false)} dismissible>
        Incorrect username, email, or password.
      </Alert>
    );
  }
}

export default function SignIn(props) {
  const [show, setShow] = useState(false);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async () => {
    if (username !== "" && password !== "" && !loading) {
      console.log(username, password);
      setLoading(true);
      await axios.get("https://localhost:7237/api/sign-in", {
        params: {
          username: username,
          password: password
        }
      }).then((response) => {
        if (response.data.serialNumber === "") {
          setShow(true);
        } else {
          setShow(false);
          props.setUser(response.data);
          navigate("/");
        }
        console.log(response.data);
      }).catch((error) => {
        console.log(error);
      });
      setLoading(false);
    }
  };

  return (
    <div className="sign-in">
      <Card>
        <Card.Header>Registered Voter Sign In</Card.Header>
        <Card.Body>
          <Form>
            <Failed show={show} setShow={setShow} />
            <Form.Group className="username">
              <Form.Label>Username/email:</Form.Label>
              <Form.Control
                type="username"
                placeholder="Enter your username or email address"
                onChange={(event) => (setUsername(event.target.value))} />
            </Form.Group>
            <Form.Group className="password">
              <Form.Label>Password:</Form.Label>
              <Form.Control
                type="password"
                placeholder="Enter your password"
                onChange={(event) => (setPassword(event.target.value))} />
            </Form.Group>
            <Form.Group className="checkbox">
              <Form.Check
                type="checkbox"
                label="Remember me"
                value={props.remember}
                checked={props.remember}
                onChange={() => props.setRemember(!props.remember)} />
            </Form.Group>
            <Form.Group className="submit">
              <Button varient="primary" onClick={handleSubmit}>
                Submit{" "}{(loading) ? <Spinner animation="border" size="sm" /> : undefined}
              </Button>
            </Form.Group>
            <Form.Text>
              Must be a registered voter to sign in.<br/>
              For more information on voter registration, visit <a href="https://vote.gov/">vote.gov</a>
            </Form.Text>
          </Form>
        </Card.Body>
      </Card>
    </div>
  );
}