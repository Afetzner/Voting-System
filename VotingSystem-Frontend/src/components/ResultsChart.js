import { Chart, BarSeries, ArgumentAxis, ValueAxis } from "@devexpress/dx-react-chart-bootstrap4";
import { Animation } from "@devexpress/dx-react-chart";
import "@devexpress/dx-react-chart-bootstrap4/dist/dx-react-chart-bootstrap4.css";

export default function ResultsChart(props) {
  // console.log("DISPLAY " + props.display);
  if (props.display) {
    // console.log(props.data);
    return (
      <div>
        <Chart data={props.data}>
          <ArgumentAxis />
          <ValueAxis max={7} />
          <BarSeries valueField="population" argumentField="year" />
          <Animation />
        </Chart>
      </div>
    );
  }
}