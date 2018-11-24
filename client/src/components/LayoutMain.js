import React, { Component } from 'react';
import { Heatmap } from './Heatmap';
import SessionList from './SessionList';
import Chart from './Chart';

import '../styles/layoutMain.css';

class LayoutMain extends Component {

  handleClick = e => {
    e.preventDefault();
    console.log(e);
  };

  render() {
    return (
      <div className="main-container">
        <h2 className="title player-name">Intermediate</h2>
        <div className="left-container">
          <div className="title box">
            <p className="bold">Game Sessions</p>
            <hr />
            <SessionList handleClick={this.handleClick} />
            <div />
          </div>
          <div className="box title bold sugession">Some Sugessions</div>
        </div>
        <div className="right-container">
          <Heatmap />
          <div className="chart align-center">
            <Chart />
          </div>
        </div>
      </div>
    );
  }
}

export default LayoutMain;
