import * as React from 'react';
import { scaleLinear } from 'd3-scale';

import '../styles/heatMap.css';

// All the magic numbers ✨
const SIDE_LENGTH = 105;
const HORIZONTAL_OFFSET = SIDE_LENGTH;
const HORIZONTAL_START = -77940; // Map X min coordinate
const HORIZONTAL_END = 78080; // Map X max coordinate
const HORIZONTAL_CLAMP = 650;
const VERTICAL_START = -75040; // Map Y min coordinate
const VERTICAL_END = 75060; // Map Y max coordinate
const HEXAGON_ANGLE = 0.523598776; // 30 degrees in radians
const VERTICAL_OFFSET = -Math.sin(HEXAGON_ANGLE) * SIDE_LENGTH;
const VERTICAL_CLAMP = 500; // 700

export class Heatmap extends React.Component {
  canvas = null;

  componentDidMount() {
    this.canvas && this.drawMap();
    console.log(this.props.movementData1);
    this.props.movementData1 !== undefined && this.drawPlayers();
  }

  componentDidUpdate(prevProps) {
    if (prevProps !== this.props) {
      this.drawMap();
      this.drawPlayers();
    }
  }

  drawMap = () => {
    const context = this.canvas.getContext('2d');
    const hexHeight = Math.sin(HEXAGON_ANGLE) * SIDE_LENGTH;
    const hexRadius = Math.cos(HEXAGON_ANGLE) * SIDE_LENGTH;
    const hexRectangleHeight = SIDE_LENGTH + 2 * hexHeight;
    const hexRectangleWidth = 2 * hexRadius;
    context.strokeStyle = '#CCCCCC';
    context.setLineDash([15, 15]);
    context.lineWidth = 1;

    const drawHexagon = (canvasContext, x, y) => {
      canvasContext.beginPath();
      canvasContext.moveTo(x + hexRadius, y);
      canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight);
      canvasContext.lineTo(x + hexRectangleWidth, y + hexHeight + SIDE_LENGTH);
      canvasContext.lineTo(x + hexRadius, y + hexRectangleHeight);
      canvasContext.lineTo(x, y + SIDE_LENGTH + hexHeight);
      canvasContext.lineTo(x, y + hexHeight);
      canvasContext.closePath();
      canvasContext.stroke();
    };

    const drawMap = context => {
      for (let i = 0; i < 2; ++i) {
        // draw first row
        drawHexagon(
          context,
          i * hexRectangleWidth + hexRadius + HORIZONTAL_OFFSET,
          SIDE_LENGTH + hexHeight + VERTICAL_OFFSET
        );
      }

      for (let i = 0; i < 3; ++i) {
        // draw second row
        drawHexagon(
          context,
          i * hexRectangleWidth + HORIZONTAL_OFFSET,
          2 * (SIDE_LENGTH + hexHeight) + VERTICAL_OFFSET
        );
      }
    };

    for (let i = 0; i < 2; ++i) {
      // draw last row
      drawHexagon(
        context,
        i * hexRectangleWidth + hexRadius + HORIZONTAL_OFFSET,
        3 * (SIDE_LENGTH + hexHeight) + VERTICAL_OFFSET
      );
    }

    drawMap(context);
  };

  drawPlayers = () => {
    const { movementData1 } = this.props;
    const scaleWidth = scaleLinear()
    .domain([HORIZONTAL_START, HORIZONTAL_END])
    .range([HORIZONTAL_OFFSET, HORIZONTAL_CLAMP]);
    const scaleHeight = scaleLinear()
    .domain([VERTICAL_START, VERTICAL_END])
    .range([-VERTICAL_OFFSET / 2 + SIDE_LENGTH, VERTICAL_CLAMP])
    .clamp(true);
    const context = this.players.getContext('2d');
    movementData1.length !== 0 && movementData1.forEach(item => {
      context.fillStyle = 'red';
      context.fillRect(
        scaleWidth(item.POSITION_X, 0),
        scaleHeight(item.POSITION_Y, 0),
        10,
        10,
        );
        context.fill();
    });
  };
  

  render() {
    return (
      <div className="heatmap align-center">
        <div className="align-center">
          <span className="title bold heat-map-title">Died Location</span>
        </div>
        <canvas
          width={800}
          height={700}
          ref={ref => (this.canvas = ref)}
          style={{ background: '#00000040' }}
        />
        <canvas
          width={800}
          height={700}
          ref={ref => (this.players = ref)}
          style={{ position: 'absolute', left: 0 }}
        />
      </div>
    );
  }
}
