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

  useEffect(() => {
    console.log(radioValue, choice, voted);
  }, [radioValue, choice, voted]);

  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      setPolls(response.data);
      setRadioValue(Array(response.data.length).fill(""));
      setChoice(Array(response.data.length).fill(0));
      setVoted(Array(response.data.length).fill(false));
      setDisplay(Array(response.data.length).fill(false));
      console.log(response.data);
    }).catch((error) => {
      console.log(error);
    });
  }, []);

  useEffect(() => {
    if (props.user !== null && polls !== null) {
      const responses = async () => {
        const radioValue = Array(polls.length).fill("");
        const choice = Array(polls.length).fill(0);
        const voted = Array(polls.length).fill(false);
        for (let index = 0; index < polls.length; index++) {
          console.log(props.user.serialNumber, polls[index].serialNumber);
          await axios.get("https://localhost:7237/api/voterIssueBallot", {
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
    axios.post("https://localhost:7237/api/vote", {
      params: {
        // props.user.serialNumber,
        // polls[index].serialNumber,
        // choice[index]
      }
    }).then((response) => {
      console.log(response);
    }).catch(error => {
      console.log(error);
    });
  };

  const handleDisplay = (index) => {
    console.log("INDEX:", index);
    console.log("DISPLAY:", display);
    // if (display[index] === false) {
      // setDisplay([
      //   ...display.slice(0, index),
      //   true,
      //   ...display.slice(index + 1)
      // ]);
    // }
  };

  // useEffect(() => {
  //   console.log("DISPLAY: ", display);
  // });

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