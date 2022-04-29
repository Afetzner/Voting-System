import "./Vote.css";
import Poll from "../components/Poll";
import PopUp from "../components/PopUp";
import { Accordion, Card } from "react-bootstrap";
import { useEffect, useState } from "react";
import axios from "axios";

// const polls = [
//   {
//     title: "Trump vs. Biden",
//     endDate: new Date("2021-02-04"),
//     options: ["Trump", "Biden"]
//   },
//   {
//     title: "Brown vs. Cortez Masto",
//     endDate: new Date("2023-01-08"),
//     options: ["test1", "test2"],
//   },
//   {
//     title: "Georgia Senate - Democratic Primary",
//     endDate: new Date("2020-08-24"),
//     options: ["Warnock", "Johnson-Shealey"]
//   },
//   {
//     title: "Georgia Senate - Republican Primary",
//     endDate: new Date("2022-05-25"),
//     options: ["Walker", "Black", "King", "Saddler", "Clark", "McColumn"]
//   }
// ];

export function BalletIssue(serialNumber, startDate, endDate, title, description, options) {
  this.serialNumber = serialNumber;
  this.startDate = startDate;
  this.endDate = endDate;
  this.title = title;
  this.description = description;
  this.options = options;
}

export default function Vote() {
  const [show, setShow] = useState(false);
  const [polls, setPolls] = useState([new BalletIssue("", new Date(), new Date(), "", "", [{number: 0, title: ""}])]);

  const [inProgress, setInProgress] = useState([false]); //[poll.endDate < new Date()]
  const [counted, setCounted] = useState([false]);
  const [radioValue, setRadioValue] = useState([""]);
  const [selection, setSelection] = useState([""]);

  useEffect(() => {
    axios.get("https://localhost:7237/api/polls").then((response) => {
      const polls = [];
      for (const poll of response.data) {
        if (poll !== undefined) {
          polls.push(new BalletIssue(poll.serialNumber, new Date(poll.startDate), new Date(poll.endDate), poll.title, poll.description, poll.options));
        }
      }
      console.log(polls);
      setPolls(polls);
    }).catch(error => {
      console.log(error);
    });
  }, []);


  // useEffect(() => {
  //   pollServices.getPolls().then((response) => {
  //     setPolls(response.data);
  //     console.log(response.data);
  //   }).catch(error => {
  //     console.log(error);
  //   });
  // }, []);

  // console.log(polls);

  return (
    <>
      <PopUp title={"Alert"} show={show} setShow={setShow}>Are you sure?  This cannot be undone.</PopUp>
      <div className="div__vote-selection">
        <Card>
          <Card.Body className="card-body__vote-selection">
            <Accordion>{polls.map((item, index) => Poll(item, index, setShow, inProgress[index], setInProgress[index], counted[index], setCounted[index], radioValue[index], setRadioValue[index], selection[index], setSelection[index]))}</Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}