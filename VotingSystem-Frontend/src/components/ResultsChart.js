import { Chart, BarSeries, Title, ArgumentAxis, ValueAxis } from "@devexpress/dx-react-chart-bootstrap4";
import { Animation } from "@devexpress/dx-react-chart";
import "@devexpress/dx-react-chart-bootstrap4/dist/dx-react-chart-bootstrap4.css";

export default function ResultsChart(props) {
  console.log(props.display);
  if (props.display) {
    return (
      <div>
        <Chart data={props.data}>
          <ArgumentAxis />
          <ValueAxis max={7} />
          <BarSeries valueField="population" argumentField="year" />
          <Title text="Results"/>
          <Animation />
        </Chart>
      </div>
    );
  }
}