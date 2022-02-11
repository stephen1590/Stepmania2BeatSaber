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
            { GameDifficulty.expert, 1.1 },
            { GameDifficulty.all, 0 }
        };
        private readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString();
        private readonly string AppDir = "\\SM2BS\\";
        private readonly string OptionsFileName = "SM2BS.json";
        public OptionsHelper()
        {
            string dir = AppData + AppDir;
            JObject settings = new();
            optionsPopulate(ref settings, dir);
        }
        public void optionsPopulate(ref JObject settings, string dir)
        {
            bool newOptions = false;
            if (Directory.Exists(dir))
            {
                if (File.Exists(dir+"\\"+OptionsFileName))
                {
                    String settingsAsJSONString = Helper.ReadFile(dir,OptionsFileName);
                    if (settingsAsJSONString == string.Empty)
                    {
                        newOptions = true;
                    }
                    options = Helper.optionsFromJSONGet(settingsAsJSONString);
                }
            }
            else
            {
                Directory.CreateDirectory(dir);
                newOptions = true;
            }
            if (newOptions)
            {
                optionsNew(dir);
            }
        }
        private void optionsNew(string dir)
        {
            JObject settings = FormatJSON();
            optionsSave(ref settings, dir);
        }
        private void optionsSave(ref JObject settings, string dir)
        {
            Helper.WriteJSON(settings, dir, OptionsFileName);
        }
        public JObject FormatJSON()
        {
            return new JObject(new JProperty("_ResolveRepeats", new JValue(options.ResolveRepeats)),
                new JProperty("_ResolveConflicts", new JValue(options.ResolveConflicts)),
                new JProperty("_ApplyObstacles", new JValue(options.ApplyObstacles)),
                new JProperty("_MyGameDifficulty", new JValue(options.MyGameDifficulty)),
                new JProperty("_WIPCustomLevelsPath", new JValue(options.WIPCustomLevelsPath)));
        }
    }
}
