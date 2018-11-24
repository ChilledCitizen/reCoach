using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Junction18 {
    public class JSON_Handler {

        public JSON_Handler () { }

        //List of all raw playerstate data jsons
        List<JObject> rawPlayerStateDataList = new List<JObject> ();
        List<JObject> rawPlayerSessionDataList = new List<JObject> ();
        List<JObject> gameSessionDataList = new List<JObject> ();
        List<JObject> gameFrameDataList = new List<JObject> ();

        public void AddToPlayerStateDataList (JObject jObject) {
            rawPlayerStateDataList.Add (jObject);
        }
        public void AddToSessionDataList (JObject jObject) {
            rawPlayerSessionDataList.Add (jObject);
        }

        public void AddToGameSessionData (JObject jObject) {
            gameSessionDataList.Add (jObject);
        }
        public void AddToGameFrameData (JObject jObject) {
            gameFrameDataList.Add (jObject);
        }

        public void InitializeProcessing () {
            foreach (var o in rawPlayerStateDataList) {
                SQL_Query_Handler.MK_Query_GameFrameData (o["Frame_Number"].ToObject<long> (), o["SessionID"].ToObject<long> ());
                SQL_Query_Handler.MK_Query_GameSessionData (o["SessionID"].ToObject<long> ());
            }

            Data_Optimizer.CombineData (rawPlayerStateDataList, rawPlayerSessionDataList, gameFrameDataList, gameSessionDataList);
        }

        public List<GameFramePlayers> DeserializePlayersData (string data) {

            List<GameFramePlayers> newData = JsonConvert.DeserializeObject<List<GameFramePlayers>> (data);
            
            return newData;

        }

    }

    public class OptimizedJSON {

    }

    public class OutputJSON {

    }

    public class Viewangles {
        public double y { get; set; }
        public double w { get; set; }
        public double x { get; set; }
        public double z { get; set; }
    }

    public class Position {
        public double y { get; set; }
        public double x { get; set; }
        public double z { get; set; }
    }

    public class State2 {
        public int health { get; set; }
        public Viewangles viewangles { get; set; }
        public Position position { get; set; }
        public int fov { get; set; }
        public int weapon_id { get; set; }
        public int stateflags { get; set; }
        public int tickflags { get; set; }
    }

    public class State {
        public State2 state { get; set; }
        public int character_id { get; set; }
        public int team_id { get; set; }
    }

    public class GameFramePlayers {
        public State state { get; set; }
        public string guid { get; set; }
    }
}