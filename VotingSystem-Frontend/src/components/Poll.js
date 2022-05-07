import "./Poll.css";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ToggleButton } from "react-bootstrap";
import ResultsChart from "../components/ResultsChart";

function Result(props) {
  return (
    <Accordion flush defaultActiveKey={0} className="sub-accordion">
      <Accordion.Item eventKey={0}>
        <Accordion.Header onClick={() => props.handleRender(props.index, !props.render)}>Results</Accordion.Header>
        <Accordion.Body>
          <ResultsChart poll={props.poll} result={props.result} render={props.render} />
        </Accordion.Body>
      </Accordion.Item>
      <Accordion.Item eventKey={1}>
        <Accordion.Header onClick={() => props.handleRender(props.index, false)}>User Response</Accordion.Header>
        <Accordion.Body>
          <Response
            poll={props.poll}
            user={props.user}
            index={props.index}
            radioValue={props.radioValue}
            voted={props.voted}
            handleClick={props.handleClick}
            handleChange={props.handleChange} />
        </Accordion.Body>
      </Accordion.Item>
    </Accordion>
  );
}

function Response(props) {
  return (
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
                onChange={(event) => props.handleChange(event, option.number, props.index)}
                disabled={props.user === null || props.poll.isEnded || props.voted}
              >{option.title}</ToggleButton>);
            })}
          </ButtonGroup>
          <Button
            className="confirm-button"
            variant="success"
            onClick={() => props.handleClick(props.index)}
            disabled={props.user === null || props.radioValue === "" || props.poll.isEnded || props.voted}
          >Confirm Selection</Button>
        </div>
      </Form.Group>
      <Form.Text>{"End Date: " + props.poll.endDate}</Form.Text>
    </Form>
  );
}

export default function Poll(props) {
  return (
    <Accordion.Item key={props.index} eventKey={props.index}>
      <Accordion.Header onClick={() => props.handleRender(props.index, !props.render)}>
        <Container>
          <strong>{props.poll.title}</strong>
        </Container>
        <div className="badges">
          {(props.voted) && <Badge bg="primary">Vote Counted</Badge>}
          {(props.poll.isEnded) ? <Badge bg="danger">Ended</Badge> : <Badge bg="success">In Progress</Badge>}
        </div>
      </Accordion.Header>
      <Accordion.Body>
        {(props.poll.isEnded)
          ? <Result
              poll={props.poll}
              user={props.user}
              index={props.index}
              radioValue={props.radioValue}
              voted={props.voted}
              result={props.result}
              render={props.render}
              handleRender={props.handleRender}
              handleClick={props.handleClick}
              handleChange={props.handleChange} />
          : <Response
              poll={props.poll}
              user={props.user}
              index={props.index}
              radioValue={props.radioValue}
              voted={props.voted}
              handleClick={props.handleClick}
              handleChange={props.handleChange} />}
      </Accordion.Body>
    </Accordion.Item>
  );
}