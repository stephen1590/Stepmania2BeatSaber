using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum GameDifficulty
    {
        unknown,
        easy,
        normal,
        hard,
        expert,
        expertPlus,
        all
    }
    public class Options
    {
        public bool ResolveRepeats { get; set; } = true;
        public bool ResolveConflicts { get; set; } = true;
        public bool ApplyObstacles { get; set; } = true;
        public GameDifficulty MyGameDifficulty { get; set; } = GameDifficulty.all;
        public string WIPCustomLevelsPath { get; set; } = String.Empty;
        public string version = "0.0.1";
    }
    public class OptionsHelper
    {
        public Options? options { get; set; }
        public static readonly Dictionary<GameDifficulty, double> DifficultyScale = new()
        {
            { GameDifficulty.unknown, 1.0 },
            { GameDifficulty.easy, 0.7 },
            { GameDifficulty.normal, 0.8 },
            { GameDifficulty.hard, 0.9 },
            { GameDifficulty.expert, 1.0 },
            { GameDifficulty.expertPlus, 1.1 },
            { GameDifficulty.all, 0 }
        };
  
        private readonly string AppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\SM2BS\\";
        private readonly string OptionsFileName = "SM2BS.json";
        public OptionsHelper()
        {
            optionsPopulate();
        }
        public void optionsPopulate()
        {
            bool newOptions = false;
                if (File.Exists(AppDir + "\\"+OptionsFileName))
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
                optionsSave();
            }
        }
        public void optionsSave()
        {
            if(options == null)
            {
                options = new();
                Console.WriteLine("No Config found. Create a new one.");
            }
            Helper.WriteJSON(JObject.FromObject(options), AppDir, OptionsFileName);
        }
    }
}
