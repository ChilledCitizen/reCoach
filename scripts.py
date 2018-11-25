import json

import numpy as np

def transform_state(stateflags):
    '''
    Transforms state from sum-on-powers-of-2 integer to one-hot encoding array;
    Returns array:
    0) PlayerStateNone
    1) PlayerStateAlive
    2) PlayerStateShooting
    3) PlayerStateTakingDamage
    4) PlayerStateSpawned
    5) PlayerStateDespawned
    6) PlayerStateKilled
    '''
    # Copy argument
    sf = stateflags
    i = 1
    # Explicit flags
    flags = np.zeros(7)
    flags[0] = 1
    
    while sf > 0:
        if sf % 2 != 0:
            flags[i] = 1
            flags[0] = 0
        sf = sf // 2
        i += 1
    
    return flags

def get_killer_entry(player_name, session_id, one_player_state, game_frame):
    '''
    Returns entry "killer" who killed player defined by player_name in session defined by session_id
    '''
    
    # Get frame_number from one_player_state that corresponds to one player death in one session
    row = np.nonzero((one_player_state["stateflags"] // 32 == 1) & (one_player_state["sessionid"] == session_id))[0][0]
    #row = np.nonzero((transform_state(stateflags)[6] == 1) & (one_player_state["sessionid"] == session_id))[0][0]
    entry = one_player_state.iloc[row]
    frame_number = entry["framenumber"]
    
    # Get row from game_frame that corresponds to the frame
    row = np.nonzero((game_frame["sessionid"] == session_id) & (game_frame["framenumber"] == frame_number))[0][0]
    json_entry_players = json.loads(game_frame.iloc[row]["players"])
    
    # Define the killer
    entry_killer = None
    # For every player in JSON entry for game frame
    for player_info in json_entry_players:
        # If flag "attacking/shooting" is 1, consider player as a killer
        if (int(transform_state(player_info["state"]["state"]["stateflags"])[2]) == 1):
            entry_killer = player_info
    if entry_killer == None:
        return -1
    
    # Return JSON entry with a killer
    return entry_killer

def get_basic_stats(player_name, one_player_session, one_player_state, game_frame, weapon_desc):
    '''
    Get some basic statistics;
    Inputs:
    - player_name - player's nickname
    - one_player_session - table similar to player_session, but restricted only to one player
    - one_player_state - table similar to player_state, but restricted only to one player
    '''
    tpl = []
        
    # Session duration, accuracy, kill distance
    tpl.append(np.average(one_player_session["session_duration_seconds"]))
    tpl.append(np.average(one_player_session.iloc[np.nonzero(one_player_session["accuracy"])]["accuracy"]))
    tpl.append(np.average(one_player_session.iloc[np.nonzero(one_player_session["average_kill_distance"])]["average_kill_distance"]))

    # Get sessions where player died
    session_ids_player_died = np.asarray(one_player_state[one_player_state["stateflags"] // 32 == 1]["sessionid"])
    #session_ids_player_died = np.asarray(one_player_state[transform_state(stateflags)[6] == 1]["sessionid"])
    
    # Get weapons by which player was killed
    weapons = []
    for session_id in session_ids_player_died:
        killer_entry = get_killer_entry(player_name, session_id, one_player_state, game_frame)
        if killer_entry == -1:
            weapons.append(-1)
        else:
            weapon_id = killer_entry["state"]["state"]["weapon_id"]
            weapon_name = weapon_desc[weapon_desc["id"] == weapon_id]["name"].iloc[0]
            weapons.append(weapon_name)

    tpl.append(weapons)
    return tpl
