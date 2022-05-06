import "./Vote.css";
import Poll from "../components/Poll";
import Confirmation from "../components/Confirmation";
import { Accordion, Card, Spinner } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

export default function Vote(props) {
  const [show, setShow] = useState(false);
  const [polls, setPolls] = useState(null);
  const [radioValue, setRadioValue] = useState([]);
  const [choice, setChoice] = useState([]);
  const [voted, setVoted] = useState([]);
  const [index, setIndex] = useState(null);
  const [display, setDisplay] = useState([]);
  const [result, setResult] = useState([]);

  useEffect(() => {
    console.log(radioValue, choice, voted);
  }, [radioValue, choice, voted]);

  /* Retrieve all ballot issues */
  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      setPolls(response.data);
      setRadioValue(Array(response.data.length).fill("")); // Initialize state arrays
      setChoice(Array(response.data.length).fill(0));
      setVoted(Array(response.data.length).fill(false));
      setDisplay(Array(response.data.length).fill(false));
      setResult(Array(response.data.length).fill([]));
      console.log(response.data);
    }).catch((error) => {
      console.log(error);
    });
  }, []);

  /* Retrieve all ballot issue responses of currently signed in user */
  useEffect(() => {
    if (props.user !== null && polls !== null) {
      const responses = async () => {
        const radioValue = Array(polls.length).fill("");
        const choice = Array(polls.length).fill(0);
        const voted = Array(polls.length).fill(false);
        for (let index = 0; index < polls.length; index++) {
          console.log(props.user.serialNumber, polls[index].serialNumber);
          await axios.get("https://localhost:7237/api/voterIssueBallot", { // Wait for response before making next call
            params: {
              voterSerial: props.user.serialNumber,
              issueSerial: polls[index].serialNumber
            }
          }).then((response) => {
            if (response.data !== -1) {
              radioValue[index] = `radio-${index}${response.data}`;
              choice[index] = response.data;
              voted[index] = true;
            }
            console.log(response);
          }).catch((error) => {
            console.log(error);
          });
        }
        setRadioValue(radioValue);
        setChoice(choice);
        setVoted(voted);
      };
      responses();
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [polls]);

  /* Clear state arrays on user sign out */
  useEffect(() => {
    if (props.user === null) {
      setRadioValue([]);
      setChoice([]);
      setVoted([]);
    }
  }, [props.user]);

  const handleChange = (event, number, index) => {
    setRadioValue([
      ...radioValue.slice(0, index),
      event.currentTarget.value,
      ...radioValue.slice(index + 1)
    ]);
    setChoice([
      ...choice.slice(0, index),
      number,
      ...choice.slice(index + 1)
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
    axios.get("https://localhost:7237/api/vote", {
      params: {
        userSerialNumber: props.user.serialNumber,
        issueSerialNumber: polls[index].serialNumber,
        choice: choice[index]
      }
    }).then((response) => {
      console.log(response);
    }).catch(error => {
      console.log(error);
    });
  };

  const sleep = (ms) => {
    return new Promise((resolve) => setTimeout(resolve, ms));
  };

  const handleDisplay = (index) => {
    sleep(150).then(() => {
      if (polls[index].isEnded) {
        const buffer = Array(polls.length).fill(false);
        buffer[index] = !display[index];
        setDisplay(buffer);
      }
    });
  };

  useEffect(() => {
    console.log(display);
  }, [display]);

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
              {(polls !== null) ? polls.map((item, index) =>
                <Poll
                  key={index}
                  poll={item}
                  user={props.user}
                  index={index}
                  radioValue={radioValue[index]}
                  voted={voted[index]}
                  handleClick={handleClick}
                  handleChange={handleChange}
                  display={display[index]}
                  handleDisplay={handleDisplay}
                />) : <><Spinner animation="border" size="sm" />{" "}Loading...</>}
            </Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}