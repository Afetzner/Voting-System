import "./Poll.css";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ToggleButton } from "react-bootstrap";
import ResultsChart from "../components/ResultsChart";

function Result(props) {
  const data = [
    { year: "1950", population: 2.525 },
    { year: "1960", population: 3.018 },
    { year: "1970", population: 3.682 },
    { year: "1980", population: 4.440 },
    { year: "1990", population: 5.310 },
    { year: "2000", population: 6.127 },
    { year: "2010", population: 6.930 },
  ];

  return (
    <Accordion flush defaultActiveKey={0} className="sub-accordion">
      <Accordion.Item eventKey={0}>
        <Accordion.Header>Results</Accordion.Header>
        <Accordion.Body>
          <ResultsChart data={data} render={props.render} />
        </Accordion.Body>
      </Accordion.Item>
      <Accordion.Item eventKey={1}>
        <Accordion.Header>User Response</Accordion.Header>
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
      <Accordion.Header onClick={() => props.handleRender(props.index)}>
        <Container>
          <strong>{props.poll.title}</strong>
        </Container>
        <div className="badges">
          {(props.voted) ? <Badge bg="primary">Vote Counted</Badge> : undefined}
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
              handleClick={props.handleClick}
              handleChange={props.handleChange}
              render={props.render} />
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