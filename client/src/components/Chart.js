import React from 'react';
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend
} from 'recharts';

const Chart = (props) => {
  const data1 = [];
  const {data} = props;
  data.forEach(session => data1.push({'SurvivedDuration': session.SESSION_TIME }))
  return (
    <div>
      <div className="align-center">
        <span className="title bold heat-map-title">Game Survived Chart</span>
      </div>
      <LineChart
        width={600}
        height={300}
        data={data1}
        margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
      >
        <XAxis />
        <YAxis />
        <CartesianGrid strokeDasharray="3 3" />
        <Tooltip />
        <Legend />
        <Line
          type="monotone"
          dataKey="SurvivedDuration"
          stroke="#8884d8"
          activeDot={{ r: 8 }}
        />
      </LineChart>
    </div>
  );
};

export default Chart;
