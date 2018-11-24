using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Junction18 {
    public static class SQL_Query_Handler {
        static JSON_Handler jsonHandler = new JSON_Handler ();
        static string connectionString = "Data Source=/home/cask/Desktop/Junction18/DarwinProject_Files/TheDarwinProject-demo.db;Version=3;";

        //Making the query
        public static void MK_Query (string playerGUID) {

            //Need reference to db, should it be a parameter?

            using (SQLiteConnection con = new SQLiteConnection (connectionString, true)) {

                con.Open ();

                SQLiteCommand command = new SQLiteCommand ();
                SQLiteDataReader dataReader;

                //Getting player state on the frame they died - will have more frames taken before death later
                command.CommandText = "SELECT * FROM player_state WHERE stateflags > 32 AND playerguid = '" + playerGUID + "'";
                command.CommandType = CommandType.Text;
                command.Connection = con;

                dataReader = command.ExecuteReader ();
                JObject rawData = new JObject ();

                while (dataReader.Read ()) {
                    rawData = new JObject (
                        new JProperty ("SessionID", dataReader["sessionid"]),
                        new JProperty ("Position_X", dataReader["posX"]),
                        new JProperty ("Position_Y", dataReader["posY"]),
                        new JProperty ("Frame_Number", dataReader["framenumber"]),
                        new JProperty ("Player_Movement_State", dataReader["tickflags"]),
                        new JProperty ("Player_State", dataReader["stateflags"]),
                        new JProperty ("Health", dataReader["health"]),
                        new JProperty ("WeaponID", dataReader["weapon_id"])
                    );

                    jsonHandler.AddToPlayerStateDataList (rawData);
                }
                dataReader.Close ();

                //Getting player session data
                command.CommandText = "SELECT * FROM player_session WHERE playerguid = '" + playerGUID + "'";
                command.CommandType = CommandType.Text;

                dataReader = command.ExecuteReader ();
                while (dataReader.Read ()) {
                    rawData = new JObject (
                        new JProperty ("SessionID", dataReader["sessionid"]),
                        new JProperty ("Session_Duration_In_Seconds", dataReader["session_duration_seconds"]),
                        new JProperty ("Kills", dataReader["kills"]),
                        new JProperty ("Deaths", dataReader["deaths"]),
                        new JProperty ("Longest_Kill_Streak", dataReader["longest_kill_streak"]),
                        new JProperty ("Accuracy", dataReader["accuracy"])

                    );

                    jsonHandler.AddToSessionDataList (rawData);
                }

                dataReader.Close ();
                command.Dispose ();

                con.Close ();

            }
            jsonHandler.InitializeProcessing ();
        }

        public static string GetWeaponName (int id) {
            using (SQLiteConnection con = new SQLiteConnection (connectionString, true)) {

                con.Open ();

                SQLiteCommand command = new SQLiteCommand ();
                SQLiteDataReader dataReader;

                command.CommandText = "SELECT * FROM weapon_desc WHERE id = " + id;
                command.CommandType = CommandType.Text;
                command.Connection = con;

                dataReader = command.ExecuteReader ();
                string info = "";
                while (dataReader.Read ()) {
                    info = dataReader["name"].ToString ();
                }
                dataReader.Close ();
                command.Dispose ();
                con.Close ();

                return info;
            }

        }
        public static void MK_Query_GameFrameData (long framenumber, long sessionid) {
            using (SQLiteConnection con = new SQLiteConnection (connectionString, true)) {

            con.Open ();

            SQLiteCommand command = new SQLiteCommand ();
            SQLiteDataReader dataReader;

            command.CommandText = "SELECT * FROM game_frame WHERE framenumber = " + framenumber + " AND sessionid = " + sessionid;
            command.CommandType = CommandType.Text;
            command.Connection = con;

            dataReader = command.ExecuteReader ();

            //Magic
            JObject rawData = new JObject ();

            while (dataReader.Read ()) {
            rawData = new JObject (
            new JProperty ("SessionID", dataReader["sessionid"]),
            new JProperty ("Frame_Number", dataReader["framenumber"]),
            new JProperty ("Players", dataReader["players"]));
            jsonHandler.AddToGameFrameData (rawData);
            //Console.WriteLine(rawData["Players"]);
                }

                dataReader.Close ();
                command.Dispose ();
                con.Close ();

            }
        }
        public static void MK_Query_GameSessionData (long sessionid) {
            using (SQLiteConnection con = new SQLiteConnection (connectionString, true)) {

                con.Open ();

                SQLiteCommand command = new SQLiteCommand ();
                SQLiteDataReader dataReader;

                command.CommandText = "SELECT * FROM game_session WHERE sessionid = " + sessionid;
                command.CommandType = CommandType.Text;
                command.Connection = con;

                dataReader = command.ExecuteReader ();

                //Magic
                JObject rawData = new JObject ();

                while (dataReader.Read ()) {
                    rawData = new JObject (
                        new JProperty ("SessionID", dataReader["sessionid"]),
                        new JProperty ("Time_Started", dataReader["time_started"])
                        //new JProperty("Events",dataReader["events"])
                    );
                    jsonHandler.AddToGameSessionData (rawData);
                }

                dataReader.Close ();
                command.Dispose ();
                con.Close ();

            }
        }
    }

}