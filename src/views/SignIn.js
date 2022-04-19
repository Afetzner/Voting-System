import "./SignIn.css";
import { useState } from "react";
import { Alert, Button, Form, Card } from "react-bootstrap";
import Header from "../components/Header";

function FailedSignIn() {
  const [show, setShow] = useState(true);
  if (show) {
    return (
      <Alert variant="danger" onClose={() => setShow(false)} dismissible>
        Incorrect username, email, or password.
      </Alert>
    );
  }
}

export default function SignIn() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(username, password);
  }

  return (
    <>
      <Header />
      <div className="div__sign-in">
        <Card>
          <Card.Header>Registered Voter Sign In</Card.Header>
          <Card.Body className="card-body__sign-in">
            <Form>
              <FailedSignIn />
              <Form.Group className="form-group__username">
                <Form.Label>Username/email:</Form.Label>
                <Form.Control type="username" placeholder="Enter your username or email address" onChange={(e) => {setUsername(e.target.value)}} />
              </Form.Group>
              <Form.Group className="form-group__password">
                <Form.Label>Password:</Form.Label>
                <Form.Control type="password" placeholder="Enter your password" onChange={(e) => (setPassword(e.target.value))} />
              </Form.Group>
              <Form.Group className="form-group__check">
                <Form.Check type="checkbox" label="Remember me"/>
                </Form.Group>
              <Form.Group className="form-group__submit">
                <Button varient="primary" onClick={handleSubmit}>Submit</Button>
              </Form.Group>
              <Form.Text>
                Must be a registered voter to sign in.<br/>
                For more information on voter registration, visit <a href="https://vote.gov/">vote.gov</a>
              </Form.Text>
            </Form>
            </Card.Body>
          </Card>
      </div>
    </>
  );
}
