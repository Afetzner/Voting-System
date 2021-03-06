import { Button, Modal } from "react-bootstrap";

export default function Confirmation(props) {
  return (
    <>
      <Modal show={props.show} onHide={() => props.setShow(false)}>
        <Modal.Header closeButton>
          <Modal.Title>{props.title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>{props.children}</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => props.setShow(false)}>Cancel</Button>
          <Button variant="primary" onClick={() => props.handleConfirmation()}>Confirm</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}