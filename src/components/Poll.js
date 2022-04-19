import "./Poll.css";
import React, { useState } from "react";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ProgressBar, ToggleButton } from "react-bootstrap";
import { Modal } from "bootstrap";
import Confirmation from "../components/Confirmation";

export default function Poll(i, poll) {
  const [inProgress, setInProgress] = useState(poll.endDate < new Date());
  const [radioValue, setRadioValue] = useState("");
  // const [selection, setSelection] = useState("");

  // function Result() {
  //   if (!inProgress) {
  //     return (
  //       <Form.Group>
  //         {/* Election Results:
  //         <div style={{paddingBottom: "8px"}}>
  //           <ProgressBar style={{height: "40px", fontSize: "20px", fontWeight: "bold"}} variant="primary" now={40} label={`Trump @ ${40}%`} />
  //           <ProgressBar style={{height: "40px"}} variant="danger" now={20} label={`${20}%`} />
  //         </div>
  //         Winner: Trump */}
  //         Winner: test1
  //         <ProgressBar style={{height: "40px", fontSize: "20px", fontWeight: "bold"}}>
  //           <ProgressBar variant="primary" now={40} label={"40%"}></ProgressBar>
  //           <ProgressBar variant="danger" now={20} label={"20%"}></ProgressBar>
  //           <ProgressBar variant="success" now={20} label={"20%"}></ProgressBar>
  //           <ProgressBar variant="warning" now={5} label={"5%"}></ProgressBar>
  //         </ProgressBar>
  //       </Form.Group>
  //     )
  //   }
  // }

  function Confirmation() {
    const [show, setShow] = useState(false);

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);

    return (
      <>
        <Modal show={show} onHide={handleClose}>
          <Modal.Header>
            <Modal.Title>Test</Modal.Title>  
          </Modal.Header>
          <Modal.Body>Are you sure?</Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={handleClose}>Cancel</Button>
            <Button variant="primary" onClick={handleClose}>Confirm</Button>
          </Modal.Footer>
        </Modal>
      </>
    );
  }

  return (
    <Accordion.Item key={i} eventKey={i}>
      <Accordion.Header>
        <Container>
          <strong>{poll.title}</strong>
        </Container>
        {(inProgress) ? <Badge bg="success">In Progress</Badge> : <Badge bg="danger">Ended</Badge>}
      </Accordion.Header>
      <Accordion.Body>
        <Form>
          <Form.Group>
            <div className="d-grid gap-2">
              <ButtonGroup vertical>
                {poll.choices.map((item, index) => {
                  const value = i + "" + index;
                  return (<ToggleButton
                    key={index}
                    type="radio"
                    variant="outline-primary"
                    value={value}
                    checked={(radioValue === value)}
                    onClick={() => {setRadioValue(value)}}
                    disabled={!inProgress}
                  >{item}</ToggleButton>);
                })}
              </ButtonGroup>
              <Button
                variant="success"
                style={{width: "200px"}}
                onClick={Confirmation}
                disabled={!inProgress}
              >Confirm Selection</Button>
            </div>
          </Form.Group>
          <Form.Text>{"End Date: " + poll.endDate}</Form.Text>
        </Form>
      </Accordion.Body>
    </Accordion.Item>
  );
}