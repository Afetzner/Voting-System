import { Button, Modal } from "react-bootstrap";

export default function Confirmation(props) {
  if (props.show) {
    return (
      <>
        <Modal show={props.show} onHide={() => props.setShow(false)}>
          <Modal.Header>
            <Modal.Title>Test</Modal.Title>  
          </Modal.Header>
          <Modal.Body>Are you sure?</Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => props.setShow(false)}>Cancel</Button>
            <Button variant="primary" onClick={() => props.setShow(false)}>Confirm</Button>
          </Modal.Footer>
        </Modal>
      </>
    );
  }
}