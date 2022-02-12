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
    public class BSaberNote : BSaber
    {
        public LineLayer _lineLayer { get; set; }
        public NoteType _noteType { get; set; }
        public CutDirection _cutDirection { get; set; }
        public BSaberNote()
        {
            _time = 0;
            _lineIndex = LineIndex.left;
            _lineLayer = LineLayer.bottom;
            _noteType = NoteType.red;
            _cutDirection = CutDirection.left;
        }
    }
    public class BSaberObstacle : BSaber
    {
        public ObstacleType _obstacleType { get; set; }
        public Width _width { get; set; }
        public double _duration { get; set; }

        [JsonIgnore]
        public bool _isOpen { get; set; }
        public BSaberObstacle()
        {
            _time = 0;
            _lineIndex = LineIndex.left;
            _obstacleType = ObstacleType.one;
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