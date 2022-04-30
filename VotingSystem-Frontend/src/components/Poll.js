import "./Poll.css";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ProgressBar, ToggleButton } from "react-bootstrap";

export default function Poll(props) {

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

  return (
    <Accordion.Item key={props.index} eventKey={props.index}>
      <Accordion.Header>
        <Container>
          <strong>{props.poll.title}</strong>
        </Container>
        <div className="badges">
          {(props.counted) ? <Badge bg="primary">Vote Counted</Badge> : undefined}
          {(props.inProgress) ? <Badge bg="success">In Progress</Badge> : <Badge bg="danger">Ended</Badge>}
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
                    // disabled={!props.inProgress || props.counted}
                  >{option.title}</ToggleButton>);
                })}
              </ButtonGroup>
              <Button
                className="confirm-button"
                variant="success"
                onClick={() => props.setShow(true)}
                // disabled={!props.inProgress[i] || props.counted[i] || props.selection[i] === ""}
              >Confirm Selection</Button>
            </div>
          </Form.Group>
          <Form.Text>{"End Date: " + props.poll.endDate}</Form.Text>
        </Form>
      </Accordion.Body>
    </Accordion.Item>
  );
}