using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Junction18 {
    public static class Data_Optimizer {
        static JSON_Handler json_Handler = new JSON_Handler ();
        public static void CombineData (List<JObject> playerStates, List<JObject> playerSessions, List<JObject> gameFrames, List<JObject> gameSessions) {

            List<JObject> combinedData = new List<JObject> ();
            foreach (var i in playerStates) {
                foreach (var j in playerSessions) {
                    if (i["SessionID"].ToObject<long> () == j["SessionID"].ToObject<long> ()) {

                        foreach (var k in gameFrames) {
                            if (i["Frame_Number"].ToObject<int> () == k["Frame_Number"].ToObject<int> ()) {
                                foreach (var l in gameSessions) {
                                    if (i["SessionID"].ToObject<long> () == l["SessionID"].ToObject<long> ()) {
                                        combinedData.Add (new JObject (
                                            new JProperty ("SESSION_ID", i["SessionID"]),
                                            new JProperty ("SESSION_TIME", j["Session_Duration_In_Seconds"]),
                                            new JProperty ("SESSION_START_TIME", l["Time_Started"]),
                                            new JProperty ("PLAYER_GUID", i["PlayerGUID"]),
                                            new JProperty ("HEALTH", i["Health"]),
                                            new JProperty ("KILLS", j["Kills"]),
                                            new JProperty ("DEATHS", j["Deaths"]),
                                            new JProperty ("LONGEST_KILL_STREAK", j["Longest_Kill_Streak"]),
                                            new JProperty ("ACCURACY", j["Accuracy"]),
                                            new JProperty ("PLAYER_STATE", i["Player_State"]),
                                            new JProperty ("FRAME_NUMBER", i["Frame_Number"]),
                                            new JProperty ("PLAYER_MOVEMENT_STATE", i["Player_Movement_State"]),
                                            new JProperty ("WEAPON_ID", i["WeaponID"]),
                                            new JProperty ("POSITION_X", i["Position_X"]),
                                            new JProperty ("POSITION_Y", i["Position_Y"]),
                                            new JProperty ("PLAYERS_ON_SERVER", k["Players"])

                                        ));

                                        //Console.WriteLine(combinedData);

                                    }

                                }
                            }
                        }
                    }
                }
            }
            ProcessCombinedData (combinedData);
            // Console.WriteLine(combinedData.Count);
        }

        static void ProcessCombinedData (List<JObject> data) {
            //Separate the "Players" data from the jobject to it's own object which will be processed to get 
            //the nearest player doing damage to figure out the killer and the weapon used. 
            JObject finalData = new JObject ();

            //For calculations
            int tmpK = 0;
            int tmpD = 0;
            int tmpS = 0;
            int tmpP = 0;
            int tmpA = 0;

            foreach (var session in data) {
                bool gotMurdered = false;
                tmpK += session["KILLS"].ToObject<int> ();
                tmpD += session["DEATHS"].ToObject<int> ();
                tmpS += session["SESSION_TIME"].ToObject<int> ();
                tmpA += session["ACCURACY"].ToObject<int> ();

                List<GameFramePlayers> players = json_Handler.DeserializePlayersData (session["PLAYERS_ON_SERVER"].ToString ());
                //playersPerSession.Add (players);
                //Removing duplicates
                for (int i = 0; i < players.Count - 1; i++) {
                    for (int j = i + 1; j < players.Count - 1; j++) {
                        if (players[i].guid == players[j].guid) {
                            players.Remove (players[i]);
                        }
                    }
                }
                tmpP += players.Count;
                session.Add (new JProperty ("PLACEMENT", players.Count));

                foreach (var player in players) {

                    if (player.state.state.stateflags == 3 || player.state.state.stateflags == 2) //Player shooting and alive, or only shooting
                    {
                        if (Math.Abs (player.state.state.position.x - session["POSITION_X"].ToObject<int> ()) < 1500) {
                            if (Math.Abs (player.state.state.position.y - session["POSITION_Y"].ToObject<int> ()) < 1500) {
                                if (player.guid != session["PLAYER_GUID"].ToString ()) {
                                    //the player maybe shooting the user, be the killer
                                    session.Add (new JProperty ("KILLER", player.guid));
                                    string weaponName = SQL_Query_Handler.GetWeaponName (player.state.state.weapon_id);
                                    session.Add (new JProperty ("WEAPON_KILLED_WITH", weaponName));
                                    gotMurdered = true;
                                }

                            }

                        }
                    }

                }

                session.Remove ("WEAPON_ID");
                session.Remove ("FRAME_NUMBER");
                session.Remove ("PLAYER_STATE");
                session.Remove ("PLAYERS_ON_SERVER");
                session.Remove ("PLAYER_MOVEMENT_STATE");
                session.Remove ("HEALTH");
                int time = session["SESSION_START_TIME"].ToObject<int> ();
                session.Remove ("SESSION_START_TIME");
                if (!gotMurdered) {
                    session.Add (new JProperty ("KILLER", "Environment"));
                    session.Add (new JProperty ("WEAPON_KILLED_WITH", "Cold"));
                }
                System.DateTime dtDateTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds (time).ToLocalTime ();
                session.Add ("SESSION_START_TIME", dtDateTime);

                finalData.Add (new JProperty ("Session_" + data.IndexOf (session), session));

            }

            float averageKD = 0;
            int averageSeconds = 0;
            float averageAccuracy = 0;
            int averagePlacement = 0;
            if (tmpD == 0) {
                averageKD = tmpK;
            } else if (tmpK == 0) {
                averageKD = 0;
            } else {
                averageKD = tmpK / tmpD;
            }
            if (data.Count != 0) {
                averageSeconds = tmpS / data.Count;
                averageAccuracy = tmpA / data.Count;
                averagePlacement = tmpP / data.Count;
            }

            JObject averages = new JObject (
                new JProperty ("AVERAGE_KD", averageKD),
                new JProperty ("AVERAGE_SESSION_TIME_IN_SECONDS", averageSeconds),
                new JProperty ("AVERAGE_ACCURACY", averageAccuracy),
                new JProperty ("AVERAGE_PLACEMENT", averagePlacement)
            );

            //finaData.Add (new JProperty("Averages",averages));
            string pathToData = System.IO.Directory.GetCurrentDirectory () + "/client/src/data/gameData.json";
            string pathToAverages = System.IO.Directory.GetCurrentDirectory () + "/client/src/data/gameAverages.json";
            System.IO.File.WriteAllText (pathToData, finalData.ToString ());
            System.IO.File.WriteAllText (pathToAverages, averages.ToString ());

        }

    }
}