import { Chart, BarSeries, ArgumentAxis, ValueAxis } from "@devexpress/dx-react-chart-bootstrap4";
import { Animation } from "@devexpress/dx-react-chart";
import "@devexpress/dx-react-chart-bootstrap4/dist/dx-react-chart-bootstrap4.css";

export default function ResultsChart(props) {
  return (
    <div style={{height: "500px"}}>
      {(props.render) && <Chart data={props.result}>
        <ArgumentAxis />
          <ValueAxis max={props.result.length} />
        <BarSeries valueField="votes" argumentField="choice" color={props.dark && "#375a7f"}/>
        <Animation />
      </Chart>}
    </div>
  );
}