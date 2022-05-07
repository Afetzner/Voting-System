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
  const [render, setRender] = useState([]);
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
      setRender(Array(response.data.length).fill(false));
      // setResult(Array(response.data.length).fill().map(() => Array(0)));
      console.log(response);
    }).catch((error) => {
      console.log(error);
    });
  }, []);


  /* Retrieve all ballot issue responses of currently signed in user */
  useEffect(() => {
    if (props.user !== null && polls !== null) {
      const responses = async () => {
        const buffer = [
          Array(polls.length).fill(""),
          Array(polls.length).fill(0),
          Array(polls.length).fill(false)
        ];
        for (let i = 0; i < polls.length; i++) {
          console.log(props.user.serialNumber, polls[i].serialNumber);
          await axios.get("https://localhost:7237/api/voterIssueBallot", { // Wait for response before making next call
            params: {
              voterSerial: props.user.serialNumber,
              issueSerial: polls[i].serialNumber
            }
          }).then((response) => {
            if (response.data !== -1) {
              buffer[0][i] = `radio-${i}${response.data}`;
              buffer[1][i] = response.data;
              buffer[2][i] = true;
            }
            console.log(response);
          }).catch((error) => {
            console.log(error);
          });
        }
        setRadioValue(buffer[0]);
        setChoice(buffer[1]);
        setVoted(buffer[2]);
      };
      responses();
    }
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [polls]);


  /* Retrieve results for ended balloet issues */
  useEffect(() => {
    if (polls !== null) {
      const responses = async () => {
        const buffer = Array(polls.length).fill().map(() => Array(0));
        for (let i = 0; i < polls.length; i++) {
          if (polls[i].isEnded) {
            await axios.get("https://localhost:7237/api/issueResults", {
              params: {
                issueSerial: polls[i].serialNumber
              }
            }).then((response) => {
              for (let j = 0; j < response.data.length; j++) {
                buffer[i].push({ choice: polls[i].options[j].title, votes: response.data[j] });
              }
              console.log(response);
            }).catch((error) => {
              console.log(error);
            });
          }
        }
        setResult(buffer);
      };
      responses();
    }
  }, [polls]);

  useEffect(() => {
    console.log("results:", result);
  }, [result]);


  /* Clear state arrays on user sign out */
  useEffect(() => {
    if (props.user === null) {
      setRadioValue([]);
      setChoice([]);
      setVoted([]);
    }
  }, [props.user]);


  /* User selection updater */
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


  /* Handle user vote submmision */
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


  /* Results graph render controller */
  const sleep = (ms) => {
    return new Promise((resolve) => setTimeout(resolve, ms));
  };

  const handleRender = (index) => {
    sleep(150).then(() => {
      if (polls[index].isEnded) {
        const buffer = Array(polls.length).fill(false); // prevent two graphs from rendering simultaneously
        buffer[index] = !render[index];
        setRender(buffer);
      }
    });
  };

  useEffect(() => {
    console.log("render:", render);
  }, [render]);


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
                  render={render[index]}
                  handleRender={handleRender}
                  result={result[index]}
                />) : <><Spinner animation="border" size="sm" />{" "}Loading...</>}
            </Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}