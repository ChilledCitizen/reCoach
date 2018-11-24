using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Junction18 {
    public static class Data_Optimizer {
        public static void CombineData (List<JObject> playerStates, List<JObject> playerSessions, List<JObject> gameFrames, List<JObject> gameSessions) {

            JObject combinedData = new JObject ();
            foreach (var i in playerStates) {
                foreach (var j in playerSessions) {
                    if (i["SessionID"].ToObject<long> () == j["SessionID"].ToObject<long> ()) {

                        foreach (var k in gameFrames) {
                            if (i["Frame_Number"].ToObject<int> () == k["Frame_Number"].ToObject<int> ()) {
                                foreach (var l in gameSessions) {
                                    if(i["SessionID"].ToObject<long>() == l["SessionID"].ToObject<long>())
                                    {
                                        combinedData = new JObject(
                                            new JProperty("SessionID",i["SessionID"]),
                                            new JProperty("SessionTime",j["Session_Duration_In_Seconds"]),
                                            new JProperty("SessionStartTime",l["Time_Started"]),
                                            new JProperty("Health",i["Health"]),
                                            new JProperty("Kills",j["Kills"]),
                                            new JProperty("Deaths",j["Deaths"]),
                                            new JProperty("Longest_Kill_Streak",j["Longest_Kill_Streak"]),
                                            new JProperty("Accuracy",j["Accuracy"]),
                                            new JProperty("PlayerState",i["Player_State"]),
                                            new JProperty("FrameNumber", i["Frame_Number"]),
                                            new JProperty("PlayerMovementState",i["Player_Movement_State"]),
                                            new JProperty("WeaponID",i["WeaponID"]),
                                            new JProperty("PositionX",i["Position_X"]),
                                            new JProperty("PositionY",i["Position_Y"]),
                                            new JProperty("PlayersOnServer",k["Players"])
                                            

                                        );
                                        ProcessCombinedData(combinedData);
                                        Console.WriteLine(combinedData);

                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    
        static void ProcessCombinedData(JObject data)
        {
            //Separate the "Players" data from the jobject to it's own object which will be processed to get 
            //the nearest player doing damage to figure out the killer and the weapon used. 

            string playersData = data["PlayersOnServer"].ToString();

            
        }
    
    }
}