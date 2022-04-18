import "./Poll.css";
import React, { useState } from "react";
import { Accordion, Badge, Container, ToggleButton, Form } from "react-bootstrap";



export default function Poll(i, poll) {
  const [inProgress, setInProgress] = useState(poll.endDate < new Date());

  function Body() {
    return (
      <>
        <Form.Group>
          <div className="d-grid gap-2">
            {poll.choices.map((item) => {return (<ToggleButton>{item}</ToggleButton>)})}  
         </div>
        </Form.Group>
        <Form.Text>{"End Date: " + poll.endDate}</Form.Text>
      </>
    );
  }

  return (
    <Accordion.Item eventKey={i}>
      <Accordion.Header>
        <Container>
          <strong>{poll.title}</strong>
        </Container>
        {(inProgress) ? <Badge bg="success">In Progress</Badge> : <Badge bg="danger">Ended</Badge>}
      </Accordion.Header>
      <Accordion.Body>
        <Form>
          <Body />
        </Form>
      </Accordion.Body>
    </Accordion.Item>
  );
  
/*
  return (
    <>
      <Accordion.Item eventKey={i}>
        <Accordion.Header>
          <Container>
            <strong>{poll.title}</strong>
          </Container>
          <div className="header-badge">
            <Container>
              <Status endDate={poll.endDate}/>
            </Container>
          </div>
        </Accordion.Header>
        <Accordion.Body>
          <Form>
            <Form.Group>
              <div className="d-grid gap-2">
                <ToggleButton type="checkbox" varient="primary" checked="false" size="lg">{poll.choice1}</ToggleButton>
                <ToggleButton type="checkbox" variant="danger" checked="true" size="lg">{poll.choice2}</ToggleButton>
              </div>
            </Form.Group>
            <Form.Text>{"End Date: " + poll.endDate}</Form.Text>
          </Form>
        </Accordion.Body>
      </Accordion.Item>
    </>
  );
  */
}