import { useState } from "react";
import { Button, Modal } from "react-bootstrap";

export default function Confirmation(show, setShow) {
  // const [show, setShow] = useState(false);

  // const handleClick = (e) => {
  //   setShow(!show);
  //   console.log(show);
  // }

  if (show) {
    return (
      <>
        <Modal show={show} onHide={setShow(false)}>
          <Modal.Header>
            <Modal.Title>Test</Modal.Title>  
          </Modal.Header>
          <Modal.Body>Are you sure?</Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={setShow(false)}>Cancel</Button>
            <Button variant="primary" onClick={setShow(false)}>Confirm</Button>
          </Modal.Footer>
        </Modal>
      </>
    );
  }
}