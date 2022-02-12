using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepmania2BeatSaber
{
    public class ChromapperBase
    {
        public string _version { get; set; } = "2.0.0";
    }
    public class ChroMap : ChromapperBase
    {
        public new string _version { get; set; } = "2.2.0";
        public List<BSaberNote> _notes { get; set; } = new();
        public List<BSaberObstacle> _obstacles { get; set; } = new();
        public List<BSaberEvent> _events { get; set; } = new();
        public List<BSaberWaypoint> _waypoints { get; set; } = new();
        public ChroMap(string v, List<BSaberNote>  n, List<BSaberObstacle>  o, List<BSaberEvent> e, List<BSaberWaypoint> w)
        {
            if(v!= String.Empty)
            {
                _version = v;
            } 
            if(n != null)
            {
                _notes = n;
            }
            if(o != null)
            {
                _obstacles = o;
            }
            if(e != null)
            {
                _events = e;
            }
            if(w != null)
            {
                _waypoints = w;
            }
        }
    }
    public class ChroInfo : ChromapperBase
    {
        public new string _version { get; set; } = "";
        public string _songName { get; set; } = "";
        public string _songSubName { get; set; } = "";
        public string _songAuthorName { get; set; } = "";
        public string _levelAuthorName { get; set; } = "";
        public string _beatsPerMinute { get; set; } = "";
        public string _previewStartTime { get; set; } = "";
        public string _previewDuration { get; set; } = "";
        public string _songTimeOffset { get; set; } = "";
        public string _shuffle { get; set; } = "";
        public string _shufflePeriod { get; set; } = "";
        public string _coverImageFilename { get; set; } = "";
        public string _songFilename { get; set; } = "";
        public string _environmentName { get; set; } = "";
        public string _allDirectionsEnvironmentName { get; set; } = "";
        public string _customData { get; set; } = "";
        public string _editors { get; set; } = "";
        public string _lastEditedBy { get; set; } = "";
        public string ChroMapper { get; set; } = "";
        public string version { get; set; } = "";
        public string _difficultyBeatmapSets { get; set; } = "";
        public string _beatmapCharacteristicName { get; set; } = "";
        public string _difficultyBeatmaps { get; set; } = "";
        public string _difficulty { get; set; } = "";
        public string _difficultyRank { get; set; } = "";
        public string _beatmapFilename { get; set; } = "";
        public string _noteJumpMovementSpeed { get; set; } = "";
        public string _noteJumpStartBeatOffset { get; set; } = "";
    }

}
