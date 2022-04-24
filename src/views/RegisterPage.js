import "./RegistrationPage.css";
import { useState } from "react";
import { Alert, Button, Form, Card } from "react-bootstrap";
import Header from "../components/Header";

function FailedRegister() {
  const [show, setShow] = useState(true);
  if (show) {
    return (
      <Alert variant="danger" onClose={() => setShow(false)} dismissible>
        This email exist already.
      </Alert>
    );
  }
}

export default function Register(){
  const [first, setFirst] = useState("");
  const [last, setLast] = useState("");
  // const [birthday, setBirthday] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  
  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(first, last, username, password);
  }

  const handleChange =(e) => {
    if (e.target.value.length) {
      
    }
  }

  return (
    <><Header />
    <div className="div__sign-in">
      <Card>
        <Card.Header>Registered Voter Sign In</Card.Header>
        <Card.Body className="card-body__sign-in">
          <Form>
            <FailedRegister />
            <Form.Group className="form-group__first">
              <Form.Label>Firstname</Form.Label>
              <Form.Control type="first" placeholder="Enter your firstname" onChange={(e) => { setFirst(e.target.value); } } />
            </Form.Group>
            <Form.Group className="form-group__last">
              <Form.Label>Lastname:</Form.Label>
              <Form.Control type="last" placeholder="Enter your lastname" onChange={(e) => { setLast(e.target.value); } } />
            </Form.Group>
            {/* <Form.Group className="form-group__birthday">
              <Form.Label>Lastname:</Form.Label>
              <Form.Control type="lastname" placeholder="Enter your birth date" onChange={(e) => { setBirthday(e.target.value); } } />
            </Form.Group> */}
            <Form.Group className="form-group__username">
              <Form.Label>Username/email:</Form.Label>
              <Form.Control type="username" placeholder="Enter your username or email address" onChange={(e) => { setUsername(e.target.value); } } />
            </Form.Group>
            <Form.Group className="form-group__password">
              <Form.Label>Password:</Form.Label>
              <Form.Control type="password" placeholder="Enter your password" onChange={(e) => (setPassword(e.target.value))} />
            </Form.Group>
            <Form.Group className="form-group__submit">
              <Button varient="primary" onClick={handleSubmit}>Submit</Button>
            </Form.Group>
          </Form>
        </Card.Body>
      </Card>
    </div></>
  );
}
