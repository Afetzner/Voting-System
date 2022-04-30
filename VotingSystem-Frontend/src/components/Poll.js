import "./Poll.css";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ToggleButton } from "react-bootstrap";

export default function Poll(props) {
  console.log("USER", props.user);
  return (
    <Accordion.Item key={props.index} eventKey={props.index}>
      <Accordion.Header>
        <Container>
          <strong>{props.poll.title}</strong>
        </Container>
        <div className="badges">
          {(props.counted) ? <Badge bg="primary">Vote Counted</Badge> : undefined}
          {(props.poll.isEnded) ? <Badge bg="danger">Ended</Badge> : <Badge bg="success">In Progress</Badge>}
        </div>
      </Accordion.Header>
      <Accordion.Body>
        <Form>
          <Form.Group>
            <div className="d-grid gap-2">
              <ButtonGroup vertical>
                {props.poll.options.map((option, index) => {
                  const value = `radio-${props.index}${index}`;
                  return (<ToggleButton
                    key={index}
                    id={value}
                    type="radio"
                    variant="outline-primary"
                    value={value}
                    checked={(props.radioValue === value)}
                    onChange={(event) => props.handleChange(event, option, props.index)}
                    disabled={props.user === undefined || props.poll.isEnded}
                  >{option.title}</ToggleButton>);
                })}
              </ButtonGroup>
              <Button
                className="confirm-button"
                variant="success"
                onClick={() => props.setShow(true)}
                disabled={props.user === undefined || props.radioValue === undefined || props.poll.isEnded}
              >Confirm Selection</Button>
            </div>
          </Form.Group>
          <Form.Text>{"End Date: " + props.poll.endDate}</Form.Text>
        </Form>
      </Accordion.Body>
    </Accordion.Item>
  );
}