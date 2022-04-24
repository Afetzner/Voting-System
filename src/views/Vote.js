import "./Vote.css";
import Poll from "../components/Poll";
import PopUp from "../components/PopUp";
import { Accordion, Card, Container } from "react-bootstrap";
import { useState } from "react";

const polls = [
  {
    title: "Trump vs. Biden",
    endDate: new Date("2021-02-04"),
    options: ["Trump", "Biden"]
  },
  {
    title: "Brown vs. Cortez Masto",
    endDate: new Date("2023-01-08"),
    options: ["test1", "test2"],
  },
  {
    title: "Georgia Senate - Democratic Primary",
    endDate: new Date("2020-08-24"),
    options: ["Warnock", "Johnson-Shealey"]
  },
  {
    title: "Georgia Senate - Republican Primary",
    endDate: new Date("2022-05-25"),
    options: ["Walker", "Black", "King", "Saddler", "Clark", "McColumn"]
  }
];

export default function Vote() {
  const [show, setShow] = useState(false);

  return (
    <>
      <PopUp title={"Alert"} show={show} setShow={setShow}>Are you sure?  This cannot be undone.</PopUp>
      <div className="div__vote-selection">
        <Container>
          <Card>
            <Card.Body className="card-body__vote-selection">
              <Accordion>{polls.map((item, index) => Poll(item, index, setShow))}</Accordion>
            </Card.Body>
          </Card>
        </Container>
      </div>
    </>
  );
}