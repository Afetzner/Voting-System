import "./Vote.css";
import Poll from "../components/Poll";
import Confimation from "../components/Confirmation";
import { Accordion, Card, Spinner } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

export default function Vote(props) {
  const [show, setShow] = useState(false);
  const [polls, setPolls] = useState(undefined);
  const [radioValue, setRadioValue] = useState([]);
  const [selection, setSelection] = useState([]);

  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      console.log(response.data);
      setPolls(response.data);
    }).catch(error => {
      console.log(error);
    });
  }, []);

  const handleChange = (event, item, index) => {
    setRadioValue([
      ...radioValue.slice(0, index),
      event.currentTarget.value,
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
      <Confimation title={"Alert"} show={show} setShow={setShow}>Are you sure?  This cannot be undone.</Confimation>
      <div className="vote-selection-container">
        <Card>
          <Card.Body className="vote-selection">
            <Accordion>
              {(polls !== undefined) ? polls.map((item, index) => 
                <Poll
                  key={index}
                  poll={item}
                  user={props.user}
                  index={index}
                  radioValue={radioValue[index]}
                  selection={selection[index]}
                  setShow={setShow}
                  handleChange={handleChange}
                />) : <><Spinner animation="border" size="sm" />{" "}Loading...</>}
            </Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}