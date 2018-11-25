import React, { Component } from 'react';
import { Heatmap } from './Heatmap';
import SessionList from './SessionList';
import Chart from './Chart';
import gameData from '../data/gameData.json';
import gameAverage from '../data/gameAverages.json';

import '../styles/layoutMain.css';

class LayoutMain extends Component {
  state = {
    SingleSessionData: '',
    Keys: Object.keys(gameData),
    data: []
  };
  componentDidMount = () => {
    const { Keys } = this.state;
    const tmp = [];
    Keys.forEach((key, i) => tmp.push(gameData[`Session_${i}`]));
    this.setState({ data: tmp });
  };
  handleClick = e => {
    e.preventDefault();
    const { data } = this.state;
    const SingleSessionData = [];
    const tmp = e.target.parentNode.id || e.target.id;
    data.forEach(session => {
      if (session.SESSION_ID == tmp) {
        SingleSessionData.push(session);
      }
      this.setState({ SingleSessionData });
    });
  };

  render() {
    const { Keys, data, SingleSessionData } = this.state;
    const { AVERAGE_KD, AVERAGE_ACCURACY, AVERAGE_SESSION_TIME_IN_SECONDS} = gameAverage;
    let playerName = '';
    data[0] !== undefined && (playerName = data[0].PLAYER_GUID);
    return (
      <div className="main-container">
        <div className="flex" >
        <h2 className="title player-name">Player: {playerName}</h2>
        <h2 className="title player-name">Avg. Kill/Death: {AVERAGE_KD}</h2>
        </div>
        <div className="flex" >
        <h2 className="title player-name">Avg. Accuracy: {AVERAGE_ACCURACY}</h2>
        <h2 className="title player-name">Avg. Survive: {AVERAGE_SESSION_TIME_IN_SECONDS}</h2>
        </div>
        <div className="left-container">
          <div className="title box">
            <p className="bold">Game Sessions</p>
            <hr />
            <SessionList
              Keys={Keys}
              data={data}
              handleClick={this.handleClick}
            />
            <div />
          </div>
          <div className="box title bold sugession">
            <p className="bold">Some Sugessions</p>
          </div>
        </div>
        <div className="right-container">
          <Heatmap movementData1={SingleSessionData} />
          <div className="chart align-center">
            <Chart data={data} />
          </div>
        </div>
      </div>
    );
  }
}

export default LayoutMain;
