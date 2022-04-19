import "./Vote.css";
import Header from "../components/Header";
import Poll from "../components/Poll";
import Confirmation from "../components/Confirmation";
import { Accordion, Card } from "react-bootstrap";

const polls = [
  {
    title: "Trump vs. Biden",
    endDate: new Date("2021-02-04"),
    choices: ["Trump", "Biden"],
    result: null
  },
  { 
    title: "Brown vs. Cortez Masto",
    endDate: new Date("2023-01-08"),
    choices: [ "test1", "test2"],
    result: null },
  { 
    title: "Georgia Senate - Democratic Primary",
    endDate: new Date("2020-08-24"),
    choices: ["Warnock", "Johnson-Shealey"],
    result: null
  },
  {
    title: "Georgia Senate - Republican Primary",
    endDate: new Date("2022-05-25"),
    choices: ["Walker", "Black", "King", "Saddler", "Clark", "McColumn"],
    result: null
  }
];

export default function Vote() {
  const rows = [];
  for (let i = 0; i < polls.length; i++) {
    rows.push(Poll(i, polls[i]));
  }
  return (
    <>
      <Header />
      <Confirmation />
      <div className="div__vote-selection">
        <Card>
          <Card.Body className="card-body__vote-selection">
            <Accordion>{rows}</Accordion>
          </Card.Body>
        </Card>
      </div>
    </>
  );
}