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

    }

    public class OptimizedJSON {

    }

    public class OutputJSON {

    }
}