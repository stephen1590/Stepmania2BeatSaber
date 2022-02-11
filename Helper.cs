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

        internal static JToken ReadJSON(string optionsFileName)
        {
            using StreamReader r = new StreamReader(optionsFileName);
            var js = r.ReadToEnd();
            string jsonString = "";
            if (js != null)
            {
                List<Options> items = JsonConvert.DeserializeObject<List<Options>>(jsonString);
                jsonString = (string)js;
            }
            return JToken.Parse(jsonString);
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
