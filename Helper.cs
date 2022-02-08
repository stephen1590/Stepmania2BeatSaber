using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    static internal class Helper
    {
        private static readonly string pChroMapperVersion = "2.2.0";
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
            Console.ForegroundColor = color;
            Console.WriteLine(outputString);
            Console.ResetColor();
        }
        public static void Output(string outputString)
        {
            Console.WriteLine(outputString);
        }
    }
}
