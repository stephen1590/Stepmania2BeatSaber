using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
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
    public class BSaberNote : BSaberBasic
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

        public new JObject ToJOject()
        {
            JObject retVal = new(new JProperty("_time", Time),
                new JProperty("_lineIndex", LineIndex),
                new JProperty("_lineLayer", LineLayer),
                new JProperty("_type", NoteType),
                new JProperty("_cutDirection", CutDirection));
            return retVal;
        }
    }
}