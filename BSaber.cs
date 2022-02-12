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
    public enum IsLeftOrRightSide
    {
        none,
        left,
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
        public BSaber()
        {
            Time = 0;
            LineIndex = LineIndex.left;
            IsLeftOrRightSide = IsLeftOrRightSide.none;
        }
        public double Time { get; set; }
        public LineIndex LineIndex { get; set; }
        public IsLeftOrRightSide IsLeftOrRightSide { get; set; }
        //public JObject ToJOject()
        //{
        //    JObject retVal = new(new JProperty("_time", Time),
        //        new JProperty("_lineIndex", LineIndex));
        //    return retVal;
        //}
    }
    public class BSaberNote : BSaber
    {
        public BSaberNote()
        {
            Time = 0;
            LineIndex = LineIndex.left;
            LineLayer = LineLayer.bottom;
            NoteType = NoteType.red;
            CutDirection = CutDirection.left;
            IsLeftOrRightSide = IsLeftOrRightSide.none;
        }
        public LineLayer LineLayer { get; set; }
        public NoteType NoteType { get; set; }
        public CutDirection CutDirection { get; set; }

        //public new JObject ToJOject()
        //{
        //    JObject retVal = new(new JProperty("_time", Time),
        //        new JProperty("_lineIndex", LineIndex),
        //        new JProperty("_lineLayer", LineLayer),
        //        new JProperty("_type", NoteType),
        //        new JProperty("_cutDirection", CutDirection));
        //    return retVal;
        //}
    }
    public class BSaberObstacle : BSaber
    {
        public BSaberObstacle()
        {
            Time = 0;
            LineIndex = LineIndex.left;
            ObstacleType = ObstacleType.one;
            IsLeftOrRightSide = IsLeftOrRightSide.none;
            IsOpen = false;
            Width = Width.one;
        }
        public ObstacleType ObstacleType { get; set; }
        public Width Width { get; set; }
        public double Duration { get; set; }
        public bool IsOpen { get; set; }
        //public new JObject ToJOject()
        //{
        //    JObject retVal = new(new JProperty("_time", Time),
        //        new JProperty("_lineIndex", LineIndex),
        //        new JProperty("_type", ObstacleType),
        //        new JProperty("_duration", Duration),
        //        new JProperty("_width", Width));
        //    return retVal;
        //}
    }
}