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
        public Options options { get; set; } = new();
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
            if (Directory.Exists(dir))
            {
                if (File.Exists(OptionsFileName))
                {
                    JToken jt = Helper.ReadJSON(OptionsFileName);
                }
                else
                {
                    JObject settings = FormatJSON();
                }
            }
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
