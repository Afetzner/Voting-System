import { Button, Modal } from "react-bootstrap";

export default function PopUp(props) {
  return (
    <>
      <Modal show={props.show} onHide={() => props.setShow(false)}>
        <Modal.Header closeButton>
          <Modal.Title>{props.title}</Modal.Title>
        </Modal.Header>
        <Modal.Body>{props.children}</Modal.Body>
        <Modal.Footer>
          <Button variant="primary" onClick={() => props.setShow(false)}>Close</Button>
        </Modal.Footer>
      </Modal>
    </>
  );
}