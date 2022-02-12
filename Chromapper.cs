using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepmania2BeatSaber
{
    public class ChromapperBase
    {
        public string _version { get; set; } = "2.2.0";
    }
    public class ChroMap : ChromapperBase
    {
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
 
}
