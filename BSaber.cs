using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum LineIndex
    {
        left,
        centerLeft,
        centerRight,
        right
    }
    public enum LineLayer
    {
        bottom,
        middle,
        top
    }
    public enum NoteType
    {
        red,
        blue
    }
    public enum CutDirection
    {
        up,
        down,
        left,
        right,
        upleft,
        upright,
        downleft,
        downright
    }
    public enum ObstacleType
    {
        one,
        two,
        three
    }
    public enum Width
    {
        none,
        one,
        two,
        three,
        four
    }
    public class BSaber
    {
        public double _time { get; set; }
        public LineIndex _lineIndex { get; set; }
        public BSaber()
        {
            _time = 0;
            _lineIndex = LineIndex.left;
        }
    }
    public class BSaberBeat
    {
        private List<BSaberNote> _notes { get;} = new();
        [JsonIgnore]
        public string mask { get; } = "";
        public double _time = 0;
        public BSaberBeat(){
        }
        public BSaberBeat(BSaberNote b)
        {
            _time = b._time;
            _notes.Add(b);
        }
        public BSaberBeat(List<BSaberNote> n)
        {
            _notes = n;
            validateBeat(null);
        }
        private void validateBeat(BSaberNote? b)
        {
            if(_time == 0)
            {
                if (b != null)
                {
                    _time = b._time;
                }
                else
                {
                    throw new NotSupportedException("Null beat found.");
                }
            }
            foreach(BSaberNote n in _notes)
            {
                if(n._time != _time)
                {
                    throw new NotSupportedException("Incorrect beat timings. These should all be on the same beat.");
                }
            }
        }
        public void addNote(BSaberNote b)
        {
            validateBeat(b);
            if (_notes.Count > 3)
                Helper.Output("Not reccomended to have 4+ notes in a single beat.");
            _notes.Add(b);
        }
        public List<BSaberNote> getNotes()
        {
            return _notes;
        }
        public int Count()
        {
            return _notes.Count;
        }
        public static bool isLike(BSaberBeat b1, BSaberBeat b2)
        {
            List<BSaberNote> b1Notes = b1.getNotes();
            List<BSaberNote> b2Notes = b2.getNotes();
            if (b1Notes.Count != b2Notes.Count)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < b2Notes.Count; i++)
                {
                    if (! b1Notes[i].isLike(b2Notes[i]))
                    {
                        return false;
                    }
                }
            }
            return true; ;
        }
    }
    public class BSaberNote : BSaber
    {
        public LineLayer _lineLayer { get; set; }
        public NoteType _type { get; set; }
        public CutDirection _cutDirection { get; set; }
        [JsonIgnore]
        public int _beatIndex { get; set; }
        public BSaberNote()
        {
            _time = 0;
            _lineIndex = LineIndex.left;
            _lineLayer = LineLayer.bottom;
            _type = NoteType.red;
            _cutDirection = CutDirection.left;
        }
        public bool isLike(BSaberNote n)
        {
            if (_lineLayer == n._lineLayer && _type == n._type && _cutDirection == n._cutDirection)
            {
                return true;
            }
            return false;
        }
    }
    public class BSaberObstacle : BSaber
    {
        public ObstacleType _type { get; set; }
        public Width _width { get; set; }
        public double _duration { get; set; }

        [JsonIgnore]
        public bool _isOpen { get; set; }
        public BSaberObstacle()
        {
            _time = 0;
            _lineIndex = LineIndex.left;
            _type = ObstacleType.one;
            _isOpen = false;
            _width = Width.one;
        }
    }
    public class BSaberEvent : BSaber
    {
        //TO DO
    }
    public class BSaberWaypoint : BSaber
    {
        //TO DO
    }
}