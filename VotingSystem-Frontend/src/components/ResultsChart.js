import { Chart, BarSeries, ArgumentAxis, ValueAxis } from "@devexpress/dx-react-chart-bootstrap4";
import { Animation } from "@devexpress/dx-react-chart";
import "@devexpress/dx-react-chart-bootstrap4/dist/dx-react-chart-bootstrap4.css";

export default function ResultsChart(props) {
  if (props.render) {
    console.log(props.result);
    return (
      <div>
        <Chart data={props.result}>
          <ArgumentAxis />
          <ValueAxis max={props.result.length} />
          <BarSeries valueField="votes" argumentField="choice" />
          <Animation />
        </Chart>
      </div>
    );
  }
}