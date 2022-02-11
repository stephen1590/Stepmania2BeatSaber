using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum DebugState
    {
        off,
        on
    }
    static internal class Helper
    {
        private static readonly string pChroMapperVersion = "2.2.0";
        public static DebugState DebugState = DebugState.off;
        public static GameDifficulty FindDifficulty(string difficulty)
        {
            switch (difficulty.Split(":")[0].Trim())
            {
                case "Beginner":
                    {
                        return GameDifficulty.easy;
                    }
                case "Easy":
                    {
                        return GameDifficulty.normal;
                    }
                case "Medium":
                    {
                        return GameDifficulty.hard;
                    }
                case "Hard":
                    {
                        return GameDifficulty.expert;
                    }
                default:
                    {
                        return GameDifficulty.unknown;
                    }
            }
        }
        public static string GameDifficultyToString(GameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case GameDifficulty.easy:
                    {
                        return "Easy";
                    }
                case GameDifficulty.normal:
                    {
                        return "Normal";
                    }
                case GameDifficulty.hard:
                    {
                        return "Hard";
                    }
                case GameDifficulty.expert:
                    {
                        return "Expert";
                    }
                default:
                    {
                        return "Unknown Difficulty";
                    }
            }
        }
        internal static JToken jTokenParse(string jsonString)
        {
            return JToken.Parse(jsonString);
        }
        internal static Options optionsFromJSONGet(string jsonString)
        {
            Options items = JsonConvert.DeserializeObject<Options>(jsonString);
            if(items == null)
            {
                items = new Options();
            }
            return items;
        }
        internal static string ReadFile(string dir, string optionsFileName)
        {
            using StreamReader r = new(dir+"\\"+optionsFileName);
            var v = r.ReadToEnd();
            string retVal = "";
            if (v != null)
            {
                retVal = (string)v;
               
            }
            return retVal;
        }
        public static void WriteJSON(JObject jOb, string dir, string filename)
        {
            // ---------
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            //----------
            using (StreamWriter writer = new(dir + "\\" + filename, false))
            {
                writer.Write(jOb.ToString());
            }
        }
        public static void WriteSongs(OrderedDictionary objectToWrite, string directory, string songName)
        {
            string basefilename = "Standard.dat";
            string newPath = directory + @"\BeatSaber - " + songName + @"\";
            foreach (GameDifficulty key in objectToWrite.Keys)
            {
                string filename = newPath + GameDifficultyToString(key) + basefilename;
                // ---------
                var difficulty = objectToWrite[key];
                if (difficulty != null)
                {
                    JArray notes = new(); 
                    var temp = ((OrderedDictionary)difficulty)["notes"];
                    if (temp != null)
                    {
                        foreach (BSaberNote n in (ArrayList)temp)
                            notes.Add(n.ToJOject());
                    }
                    JArray obstacles = new();
                    temp = ((OrderedDictionary)difficulty)["obstacles"];
                    if (temp != null)
                    {
                        foreach (BSaberObstacle o in (ArrayList)temp)
                            obstacles.Add(o.ToJOject());
                    }
                    //----------
                    JObject chroMapperJSON = FormatJSON(pChroMapperVersion, notes, obstacles, new JArray(), new JArray());
                    // ---------
                    if (!Directory.Exists(newPath))
                        Directory.CreateDirectory(newPath);
                    //----------
                    using (StreamWriter writer = new(filename, false))
                    {
                        writer.Write(chroMapperJSON.ToString());
                    }
                    Output("Wrote File: " + filename, ConsoleColor.Gray, DebugState.on);
                }
            }
        }
        public static string GetNextLine(StreamReader sr)
        {
            try
            {
                var retVal = sr.ReadLine();
                if (retVal == null)
                    return "";
                return (string)retVal;
            }
            catch
            {
                Output("Unable to get next line from reader", ConsoleColor.Red, DebugState.on);
                return "";
            }
        }
        public static JObject FormatJSON(string version, JArray notes, JArray obstacles, JArray events, JArray waypoints)
        {
            return new JObject(new JProperty("_version", version),
                new JProperty("_notes", notes),
                new JProperty("_obstacles", obstacles),
                new JProperty("_events", events),
                new JProperty("_waypoints", waypoints));
        }
        public static void Output(string outputString, ConsoleColor color)
        {
            Output(outputString, color, DebugState);
        }
        public static void Output(string outputString, ConsoleColor color, DebugState dbState)
        {
            if (dbState == DebugState.on)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(outputString);
                Console.ResetColor();
            }
        }
        public static void Output(string outputString)
        {
            Output(outputString, DebugState);
        }
        public static void Output(string outputString, DebugState dbState)
        {
            if (dbState == DebugState.on)
            {
                Console.WriteLine(outputString);
            }
        }
    }
}
