using System.Collections;
using System.Collections.Specialized;
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
        public static void WriteFile(OrderedDictionary objectToWrite, string directory, string songName)
        {
            string basefilename = "Standard.dat";
            string newPath = directory + @"\BeatSaber - " + songName + @"\";
            foreach (GameDifficulty key in objectToWrite.Keys)
            {
                string filename = newPath + GameDifficultyToString(key) + basefilename;
                // ---------
                JArray notes = new();
                var temp = objectToWrite[key];

                if (temp != null)
                {
                    foreach (BSaberNote note in (ArrayList)temp)
                        notes.Add(note.ToJOject());
                    //----------
                    JObject chroMapperJSON = FormatJSON(pChroMapperVersion, notes, new JArray());
                    // ---------
                    if (!Directory.Exists(newPath))
                        Directory.CreateDirectory(newPath);
                    //----------
                    using (StreamWriter writer = new(filename, false))
                    {
                        writer.Write(chroMapperJSON.ToString());
                    }
                    Output("Wrote File: " + filename, ConsoleColor.Cyan);
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
                Output("Unable to get next line from reader", ConsoleColor.Red);
                return "";
            }
        }
        public static JObject FormatJSON(string version, JArray notes, JArray obstacles)
        {
            return new JObject(new JProperty("_version", version),
                new JProperty("_notes", notes),
                new JProperty("_obstacles", obstacles),
                new JProperty("_events", new JArray()),
                new JProperty("_waypoints", new JArray()));
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
