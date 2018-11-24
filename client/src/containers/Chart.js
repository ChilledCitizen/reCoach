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
  { name: 'Session A', SurvivedDuration: 4000 },
  { name: 'Session B', SurvivedDuration: 3000 },
  { name: 'Session C', SurvivedDuration: 2000 },
  { name: 'Session D', SurvivedDuration: 2780 },
  { name: 'Session E', SurvivedDuration: 1890 },
  { name: 'Session F', SurvivedDuration: 2390 },
  { name: 'Session G', SurvivedDuration: 3490 }
];

const Chart = () => (
  <LineChart
    width={600}
    height={300}
    data={data}
    margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
  >
    <XAxis dataKey="name" />
    <YAxis />
    <CartesianGrid strokeDasharray="3 3" />
    <Tooltip />
    <Legend />
    <Line type="monotone" dataKey="SurvivedDuration" stroke="#8884d8" activeDot={{ r: 8 }} />
  </LineChart>
);

export default Chart;
