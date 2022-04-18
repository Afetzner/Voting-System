import "./Vote.css";
import Header from "../components/Header";
import { Accordion, Button, ToggleButton, Card, Form } from "react-bootstrap";
import Poll from "../components/Poll"

const polls = [
  { title: "Trump vs. Biden", endDate: new Date("2021-02-04"), choices: ["Trump", "Biden"], result: null },
  { title: "Brown vs. Cortez Masto", endDate: new Date("2023-01-08"), choices: ["test1", "test2"], result: null },
  { title: "Georgia Senate - Republican Primary", endDate: new Date("2020-08-24"), choices: ["test1", "test2", "test3", "test4"], result: null }
];

export default function Vote() {
  let rows = [];
  for (let i = 0; i < polls.length; i++) {
    rows.push(Poll(i, polls[i]));
  }
  return (
    <>
      <Header />
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