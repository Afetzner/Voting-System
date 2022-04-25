import "./SignIn.css";
import { useState } from "react";
import { Alert, Button, Form, Card } from "react-bootstrap";
<<<<<<< HEAD
<<<<<<< HEAD
=======
import { Link } from "react-router-dom";
>>>>>>> a86a343 (Implemented router dom and user drop down menu)

function FailedSignIn(props) {
  if (props.show) {
    return (
      <Alert variant="danger" onClose={() => props.setShow(false)} dismissible>
        Incorrect username, email, or password.
=======
import Header from "../components/Header";

function FailedSignIn(props) {
  if (props.show) {
    return (
<<<<<<< HEAD
      <Alert variant="danger" onClose={() => setShow(false)} dismissible>
<<<<<<< HEAD
      Incorrect username, email, or password.
>>>>>>> ae1b665 (Initial commit)
=======
=======
      <Alert variant="danger" onClose={() => props.setShow(false)} dismissible>
>>>>>>> 872a854 (Converted Confirm.js to a more general PopUp.js, syntax clean up)
        Incorrect username, email, or password.
>>>>>>> 10e0c32 (Continued work on Vote view)
      </Alert>
    );
  }
}

export default function SignIn() {
<<<<<<< HEAD
<<<<<<< HEAD
  const [show, setShow] = useState(true);
=======
>>>>>>> ae1b665 (Initial commit)
=======
  const [show, setShow] = useState(true);
>>>>>>> 872a854 (Converted Confirm.js to a more general PopUp.js, syntax clean up)
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(username, password);
<<<<<<< HEAD
  };

  return (
    <div className="div__sign-in">
      <Card>
        <Card.Header>Registered Voter Sign In</Card.Header>
        <Card.Body className="card-body__sign-in">
          <Form>
            <FailedSignIn show={show} setShow={setShow} />
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
  );
}
=======
  }

  return (
    <>
      <div className="div__sign-in">
        <Card>
          <Card.Header>Registered Voter Sign In</Card.Header>
          <Card.Body className="card-body__sign-in">
            <Form>
              <FailedSignIn show={show} setShow={setShow} />
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
<<<<<<< HEAD
}
>>>>>>> ae1b665 (Initial commit)
=======
}
>>>>>>> 872a854 (Converted Confirm.js to a more general PopUp.js, syntax clean up)
