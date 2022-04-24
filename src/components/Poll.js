import "./Poll.css";
import React, { useState } from "react";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ProgressBar, ToggleButton } from "react-bootstrap";

export default function Poll(poll, i, setShow) {
  const [inProgress, setInProgress] = useState(poll.endDate < new Date());
  const [counted, setCounted] = useState(false);
  const [radioValue, setRadioValue] = useState("");
  const [selection, setSelection] = useState("");

  // const [state, setState] = useState({
  //   inProgress: (poll.endDate < new Date()),
  //   counted: true,
  //   radioValue: "",
  //   selection: ""
  // });

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

  // function Confirmation() {
  //   const [show, setShow] = useState(false);

  //   const handleClose = () => setShow(false);
  //   const handleShow = () => setShow(true);

  //   return (
  //     <>
  //       <Modal show={show} onHide={handleClose}>
  //         <Modal.Header>
  //           <Modal.Title>Test</Modal.Title>  
  //         </Modal.Header>
  //         <Modal.Body>Are you sure?</Modal.Body>
  //         <Modal.Footer>
  //           <Button variant="secondary" onClick={handleClose}>Cancel</Button>
  //           <Button variant="primary" onClick={handleClose}>Confirm</Button>
  //         </Modal.Footer>
  //       </Modal>
  //     </>
  //   );
  // }

  const handleChange = (e, item) => {
    setRadioValue(e.currentTarget.value);
    setSelection(item);
    console.log(item);
  }

  return (
    <Accordion.Item key={i} eventKey={i}>
      <Accordion.Header>
        <Container>
          <strong>{poll.title}</strong>
        </Container>
        <div className="badges">
          {(counted) ? <Badge bg="primary">Vote Counted</Badge> : undefined}
          {(inProgress) ? <Badge bg="success">In Progress</Badge> : <Badge bg="danger">Ended</Badge>}
        </div>
      </Accordion.Header>
      <Accordion.Body>
        <Form>
          <Form.Group>
            <div className="d-grid gap-2">
              <ButtonGroup vertical>
                {poll.options.map((option, index) => {
                  const value = `radio-${i}${index}`;
                  return (<ToggleButton
                    key={index}
                    id={value}
                    type="radio"
                    variant="outline-primary"
                    value={value}
                    checked={(radioValue === value)}
                    onChange={(e) => handleChange(e, option)}
                    disabled={!inProgress || counted}
                  >{option}</ToggleButton>);
                })}
              </ButtonGroup>
              <Button
                className="confirm-button"
                variant="success"
                onClick={() => setShow(true)}
                disabled={!inProgress || counted || selection === ""}
              >Confirm Selection</Button>
            </div>
          </Form.Group>
          <Form.Text>{"End Date: " + poll.endDate}</Form.Text>
        </Form>
      </Accordion.Body>
    </Accordion.Item>
  );
}