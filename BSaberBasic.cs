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
    public class BSaberBasic
    {
        public BSaberBasic()
        {
            Time = 0;
            LineIndex = LineIndex.left;
            IsLeftOrRightSide = IsLeftOrRightSide.none;
        }
        public double Time { get; set; }
        public LineIndex LineIndex { get; set; }
        public IsLeftOrRightSide IsLeftOrRightSide { get; set; }
        public JObject ToJOject()
        {
            JObject retVal = new(new JProperty("_time", Time),
                new JProperty("_lineIndex", LineIndex));
            return retVal;
        }
    }
}