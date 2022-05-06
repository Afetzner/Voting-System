import "./Poll.css";
import { Accordion, Badge, Button, ButtonGroup, Container, Form, ToggleButton } from "react-bootstrap";
import ResultsChart from "../components/ResultsChart";
import { useEffect } from "react";
import { useAccordionButton } from 'react-bootstrap/AccordionButton';

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
    <Accordion flush  className="sub-accordion">
      <Accordion.Item key={`0${props.index}`} eventKey={`0${props.index}`}>
        <Accordion.Button onClick={props.handleDisplay(props.index)}>Results</Accordion.Button>
        <Accordion.Body>
          <ResultsChart data={data} display={props.display} />
        </Accordion.Body>
      </Accordion.Item>
      <Accordion.Item key={`1${props.index}`} eventKey={`1${props.index}`}>
        <Accordion.Header>User Response</Accordion.Header>
        <Accordion.Body>
          <Response poll={props.poll} user={props.user} index={props.index} radioValue={props.radioValue} voted={props.voted} handleClick={props.handleClick} handleChange={props.handleChange} />
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
                onChange={(event) => props.handleChange(event, option, props.index)}
                disabled={props.user === null || props.isAdmin === true || props.poll.isEnded || props.voted}
              >{option.title}</ToggleButton>);
            })}
          </ButtonGroup>
          <Button
            className="confirm-button"
            variant="success"
            onClick={() => props.handleClick(props.index)}
            disabled={props.user === null || props.isAdmin === true || props.radioValue === "" || props.poll.isEnded || props.voted}
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
      <Accordion.Header>
        <Container>
          <strong>{props.poll.title}</strong>
        </Container>
        <div className="badges">
          {(props.voted) ? <Badge bg="primary">Vote Counted</Badge> : undefined}
          {(props.poll.isEnded) ? <Badge bg="danger">Ended</Badge> : <Badge bg="success">In Progress</Badge>}
        </div>
      </Accordion.Header>
      <Accordion.Body>
        {(props.poll.isEnded) ? <Result poll={props.poll} user={props.user} index={props.index} radioValue={props.radioValue} voted={props.voted} handleClick={props.handleClick} handleChange={props.handleChange} display={props.display} handleDisplay={props.handleDisplay} /> : <Response poll={props.poll} user={props.user} index={props.index} radioValue={props.radioValue} voted={props.voted} handleClick={props.handleClick} handleChange={props.handleChange} />}
      </Accordion.Body>
    </Accordion.Item>
  );
}