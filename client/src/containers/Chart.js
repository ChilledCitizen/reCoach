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

const data = [
  { SurvivedDuration: 4000 },
  { SurvivedDuration: 3000 },
  { SurvivedDuration: 2000 },
  { SurvivedDuration: 2780 },
  { SurvivedDuration: 1890 },
  { SurvivedDuration: 2390 },
  { SurvivedDuration: 3490 }
];

const Chart = () => (
  <LineChart
    width={600}
    height={300}
    data={data}
    margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
  >
    <XAxis/>
    <YAxis />
    <CartesianGrid strokeDasharray="3 3" />
    <Tooltip />
    <Legend />
    <Line type="monotone" dataKey="SurvivedDuration" stroke="#8884d8" activeDot={{ r: 8 }} />
  </LineChart>
);

export default Chart;
