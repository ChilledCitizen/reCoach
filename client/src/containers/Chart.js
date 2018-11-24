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
// import json from 'json-loader';
import data from '../data/data.json';

const {sessionDurationData} = data;
const data1 = []
sessionDurationData.forEach(item=> data1.push({"SurvivedDuration":item}));


const Chart = () => {
  return(
  <LineChart
    width={600}
    height={300}
    data={data1}
    margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
  >
    <XAxis/>
    <YAxis />
    <CartesianGrid strokeDasharray="3 3" />
    <Tooltip />
    <Legend />
    <Line type="monotone" dataKey="SurvivedDuration" stroke="#8884d8" activeDot={{ r: 8 }} />
  </LineChart>
)};

export default Chart;
