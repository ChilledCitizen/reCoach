import React from 'react';

import '../styles/sessionList.css'

const SessionList = (props) =>{
    const { handleClick }= props;
    return(
    <div className="session-list__container">
        <ul className="session-list__heading align-center">
            <li>Date</li>
            <li>Total time</li>
            <li>Session ID</li>
            <li>Killed By</li>
            <li>Weapon</li>
        </ul>
        <ul className="session-list-text align-center" onClick={handleClick}>
            <li>24.11.2018</li>
            <li>100</li>
            <li>3983484839394343</li>
        </ul>
        <ul className="session-list-text align-center">
            <li>24.11.2018</li>
            <li>100</li>
            <li>3983484839394343</li>
        </ul>
    
    </div>
);}

export default SessionList;