import "./Vote.css";
import Poll from "../components/Poll";
import PopUp from "../components/PopUp";
import { Accordion, Card } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

export default function Vote() {
  const [show, setShow] = useState(false);
  const [polls, setPolls] = useState(undefined);

  const [inProgress, setInProgress] = useState([]);
  const [counted, setCounted] = useState([]);
  const [radioValue, setRadioValue] = useState([]);
  const [selection, setSelection] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      setPolls(response.data);
    }).catch(error => {
      console.log(error);
    });
  }, [polls]);

  const handleChange = (e, item, index) => {
    setRadioValue([
      ...radioValue.slice(0, index),
      e.currentTarget.value,
      ...radioValue.slice(index + 1)
    ]);
    setSelection([
      ...selection.slice(0, index),
      item,
      ...selection.slice(index + 1)
    ]);
  };

  return (
    <>
      <PopUp title={"Alert"} show={show} setShow={setShow}>Are you sure?  This cannot be undone.</PopUp>
      <div className="div__vote-selection">
        <Card>
          <Card.Body className="card-body__vote-selection">
            <Accordion>
              {(polls !== undefined) ? polls.map((item, index) => 
                <Poll
                  key={index}
                  poll={item}
                  index={index}
                  inProgress={inProgress[index]}
                  counted={counted[index]}
                  radioValue={radioValue[index]}
                  selection={selection[index]}
                  setShow={setShow}
                  handleChange={handleChange}
                />) : undefined}
            </Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}