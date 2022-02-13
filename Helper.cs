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
    public enum GameDifficulty
    {
        Unknown,
        Easy,
        Normal,
        Hard,
        Expert,
        ExpertPlus,
        All
    }
    public class Options
    {
        public bool ResolveRepeats { get; set; } = true;
        public bool ResolveConflicts { get; set; } = true;
        public bool ApplyObstacles { get; set; } = true;
        public bool TranslatePatterns { get; set; } = true;
        public GameDifficulty MyGameDifficulty { get; set; } = GameDifficulty.All;
        public string WIPCustomLevelsPath { get; set; } = String.Empty;
        public string version = "0.0.1";
    }
    static internal class Helper
    {
        public static DebugState DebugState = DebugState.off;
        public static readonly Dictionary<GameDifficulty, double> DifficultyScale = new()
        {
            { GameDifficulty.Unknown, 1.0 },
            { GameDifficulty.Easy, 0.7 },
            { GameDifficulty.Normal, 0.8 },
            { GameDifficulty.Hard, 0.9 },
            { GameDifficulty.Expert, 1.0 },
            { GameDifficulty.ExpertPlus, 1.1 },
            { GameDifficulty.All, 0 }
        };
        private static readonly string AppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SM2BS\\";
        private static readonly string OptionsFileName = "SM2BS.json";
        /* ===================================================
        * Options Handler
        =================================================== */
        public static void optionsPopulate(ref Options options)
        {
            bool newOptions = false;
            if (File.Exists(AppDir + "\\" + OptionsFileName))
            {
                String settingsAsJSONString = Helper.ReadFile(AppDir, OptionsFileName);
                if (settingsAsJSONString == string.Empty)
                {
                    newOptions = true;
                }
                options = Helper.optionsFromJSONGet(settingsAsJSONString);
                Console.WriteLine("Config succesfully loaded from AppData.");
            }
            else
            {
                newOptions = true;
            }
            if (newOptions)
            {
                //There is an automatic NEW if we have a null options
                optionsSave(ref options);
            }
        }
        public static void optionsSave(ref Options options)
        {
            if (options == null)
            {
                options = new();
                Console.WriteLine("No Config found. Create a new one.");
            }
            Helper.WriteJSON(JObject.FromObject(options), AppDir, OptionsFileName);
        }
        /* ===================================================
        * Misc
        =================================================== */
        public static GameDifficulty FindDifficulty(string difficulty)
        {
            switch (difficulty.Split(":")[0].Trim())
            {
                case "Beginner":
                    {
                        return GameDifficulty.Easy;
                    }
                case "Easy":
                    {
                        return GameDifficulty.Normal;
                    }
                case "Medium":
                    {
                        return GameDifficulty.Hard;
                    }
                case "Hard":
                    {
                        return GameDifficulty.Expert;
                    }
                default:
                    {
                        return GameDifficulty.Unknown;
                    }
            }
        }
        /* ===================================================
         * JSON and Parsing
         =================================================== */
        internal static Options optionsFromJSONGet(string jsonString)
        {
            Options items = JsonConvert.DeserializeObject<Options>(jsonString);
            if (items == null)
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
            using StreamWriter writer = new(dir + "\\" + filename, false);
            writer.Write(jOb.ToString());
        }
        public static void WriteSongs(OrderedDictionary objectToWrite, string directory, string songName)
        {
            string basefilename = "Standard.dat";
            string newPath = directory + @"\BeatSaber - " + songName + @"\";
            foreach (GameDifficulty key in objectToWrite.Keys)
            {
                string filename = ((GameDifficulty)key).ToString() + basefilename;
                // ---------
                if (objectToWrite.Contains(key))
                {
                    var difficulty = objectToWrite[key];
                    if (difficulty != null)
                    {
                        List<BSaberNote> notes = new(); 
                        var temp = ((OrderedDictionary)difficulty)["notes"];
                        if (temp != null)
                        {
                            notes = (List<BSaberNote>)temp;
                        }
                        List<BSaberObstacle> obstacles = new();
                        temp = ((OrderedDictionary)difficulty)["obstacles"];
                        if (temp != null)
                        {
                            obstacles= (List<BSaberObstacle>)temp;
                        }
                        //----------
                        ChroMap c = new("", notes, obstacles,new(),new());
                        Helper.WriteJSON(JObject.FromObject(c), newPath, filename);
                        Output("Wrote File: " + filename, ConsoleColor.Gray, DebugState.on);
                    }
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
        /* ===================================================
         * Debug output
         =================================================== */
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
