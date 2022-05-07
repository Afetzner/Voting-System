import "./Vote.css";
import Poll from "../components/Poll";
import Confirmation from "../components/Confirmation";
import { Accordion, Card, Spinner } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

export default function Vote(props) {
  const [show, setShow] = useState(false);
  const [index, setIndex] = useState(null);

  const [polls, setPolls] = useState(null);
  const [radioValues, setRadioValues] = useState([]);
  const [choices, setChoices] = useState([]);
  const [voted, setVoted] = useState([]);
  const [results, setResults] = useState([]);
  const [render, setRender] = useState([]);

  useEffect(() => {
    console.log(radioValues, choices, voted);
  }, [radioValues, choices, voted]);


  /* Retrieve all ballot issues */
  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      setPolls(response.data);
      setRadioValues(Array(response.data.length).fill("")); // Initialize state arrays
      setChoices(Array(response.data.length).fill(0));
      setVoted(Array(response.data.length).fill(false));
      setResults(Array(response.data.length).fill().map(() => Array(0)));
      setRender(Array(response.data.length).fill(false));
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
        setRadioValues(buffer[0]);
        setChoices(buffer[1]);
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
        setResults(buffer);
      };
      responses();
    }
  }, [polls]);

  useEffect(() => {
    console.log("results:", results);
  }, [results]);


  /* Clear state arrays on user sign out */
  useEffect(() => {
    if (props.user === null) {
      setRadioValues([]);
      setChoices([]);
      setVoted([]);
    }
  }, [props.user]);


  /* User selection updater */
  const handleChange = (event, number, index) => {
    setRadioValues([
      ...radioValues.slice(0, index),
      event.currentTarget.value,
      ...radioValues.slice(index + 1)
    ]);
    setChoices([
      ...choices.slice(0, index),
      number,
      ...choices.slice(index + 1)
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
        choice: choices[index]
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

  const handleRender = (index, bool) => {
    sleep(1).then(() => {
      const buffer = Array(polls.length).fill(false); // prevent two graphs from rendering simultaneously
      if (polls[index].isEnded) {
        buffer[index] = bool;
      }
      setRender(buffer);
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
                  radioValue={radioValues[index]}
                  voted={voted[index]}
                  result={results[index]}
                  render={render[index]}
                  handleRender={handleRender}
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