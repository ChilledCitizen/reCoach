import React from 'react';

import '../styles/sessionList.css';

const SessionList = props => {
  const { handleClick, data, Keys } = props;
  return (
    <div className="session-list__container">
      <ul className="session-list__heading align-center">
        <li>Date</li>
        <li>Total time</li>
        <li>Session ID</li>
        <li>Killed By</li>
        <li>Weapon</li>
      </ul>
      {data.map(session => {
        const { SESSION_ID, SESSION_TIME, SESSION_START_TIME } = session;
        return (
          <ul className="session-list-text align-center" onClick={handleClick} id= {SESSION_ID} key={SESSION_ID}>
            <li>{SESSION_START_TIME}</li>
            <li>{SESSION_TIME}</li>
            <li>{SESSION_ID}</li>
          </ul>
        );
      })}
    </div>
  );
};

export default SessionList;
