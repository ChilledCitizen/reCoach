using System;
using System.Collections.Generic;
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
                                        combinedData.Add( new JObject (
                                            new JProperty ("SessionID", i["SessionID"]),
                                            new JProperty ("SessionTime", j["Session_Duration_In_Seconds"]),
                                            new JProperty ("SessionStartTime", l["Time_Started"]),
                                            new JProperty ("Health", i["Health"]),
                                            new JProperty ("Kills", j["Kills"]),
                                            new JProperty ("Deaths", j["Deaths"]),
                                            new JProperty ("Longest_Kill_Streak", j["Longest_Kill_Streak"]),
                                            new JProperty ("Accuracy", j["Accuracy"]),
                                            new JProperty ("PlayerState", i["Player_State"]),
                                            new JProperty ("FrameNumber", i["Frame_Number"]),
                                            new JProperty ("PlayerMovementState", i["Player_Movement_State"]),
                                            new JProperty ("WeaponID", i["WeaponID"]),
                                            new JProperty ("PositionX", i["Position_X"]),
                                            new JProperty ("PositionY", i["Position_Y"]),
                                            new JProperty ("PlayersOnServer", k["Players"])

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
        }

        static void ProcessCombinedData (List<JObject> data) {
            //Separate the "Players" data from the jobject to it's own object which will be processed to get 
            //the nearest player doing damage to figure out the killer and the weapon used. 
            List<List<JObject>> playersPerServer = new List<List<JObject>>();
            foreach(var session in data)
            {
                string playersString = session["PlayersOnServer"].ToString();
                List<JObject> playersInSession = json_Handler.DeserializePlayersData(playersString);
                playersPerServer.Add(playersInSession);
            }

            


        }

    }
}