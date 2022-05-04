import "./Vote.css";
import Poll from "../components/Poll";
import Confirmation from "../components/Confirmation";
import { Accordion, Card, Spinner } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

export default function Vote(props) {
  const [show, setShow] = useState(false);
  const [polls, setPolls] = useState(undefined);
  const [radioValue, setRadioValue] = useState([]);
  const [selection, setSelection] = useState([]);
  const [voted, setVoted] = useState([]);
  const [index, setIndex] = useState();

  useEffect(() => {
    console.log(radioValue, selection, voted);
  }, [radioValue, selection, voted]);

  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      setRadioValue(Array(response.data.length).fill(""));
      setSelection(Array(response.data.length).fill(""));
      setVoted(Array(response.data.length).fill(false));
      setPolls(response.data);
    }).catch(error => {
      console.log(error);
    });
  }, []);

  useEffect(() => {
    if (props.user !== undefined && polls !== undefined) {
      const radioValue = [];
      const selection = [];
      const voted = [];
      for (let i = 0; i < polls.length; i++) {
        axios.post("https://localhost:7237/api/voted",
          props.user.serialNumber,
          polls[i].serialNumber
        ).then((response) => {
          console.log("VOTED:", response);
          if (response === null) {
            radioValue.push("");
            selection.push("");
            voted.push(false);
          } else {
            radioValue.push(`radio-${i}${response.number}`);
            selection.push(response.title);
            voted.push(true);
          }
          setRadioValue(radioValue);
          setSelection(selection);
          setVoted(voted);
        }).catch(error => {
          console.log(error);
        });
      }
    }
  }, [props.user, polls]);

  useEffect(() => {
    if (props.user === undefined) {
      setRadioValue([]);
      setSelection([]);
      setVoted([]);
    }
  }, [props.user]);

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

  const handleClick = (index) => {
    setShow(true);
    setIndex(index);
  };

  const handleConfirmation = () => {
    setShow(false);
    setVoted([
      ...voted.slice(0, index),
      true,
      ...voted.slice(index + 1)
    ]);
    axios.post("https://localhost:7237/api/vote",
      props.user.serialNumber,
      polls[index].serialNumber,
      selection[index].number,
      selection[index].title
    ).then((response) => {
      console.log(response);
    }).catch(error => {
      console.log(error);
    });
  };

  return (
    <>
      <Confirmation
        title={"Alert"}
        show={show}
        setShow={setShow}
        handleConfirmation={handleConfirmation}
      >Are you sure?  This cannot be undone.</Confirmation>
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
                  voted={voted[index]}
                  handleClick={handleClick}
                  handleChange={handleChange}
                />) : <><Spinner animation="border" size="sm" />{" "}Loading...</>}
            </Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}