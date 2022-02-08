using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum ObstacleType{
        one,
        two,
        three
    }
    public enum Width
    {
        one,
        two,
        three,
        four
    }
    public class BSaberObstacle : BSaberBasic
    {
        public BSaberObstacle()
        {
            Time = 0;
            LineIndex = LineIndex.left;
            ObstacleType = ObstacleType.one;
            CutDirection = CutDirection.left;
            IsLeftOrRightSide = IsLeftOrRightSide.none;
        }
        public ObstacleType ObstacleType { get; set; }
        public CutDirection CutDirection { get; set; }
        public Width Width { get; set; }
        public double Duration { get; set; }
        public new JObject ToJOject()
        {
            JObject retVal = new(new JProperty("_time", Time),
                new JProperty("_lineIndex", LineIndex),
                new JProperty("_type", ObstacleType),
                new JProperty("_duration", Duration),
                new JProperty("_width", Width));
            return retVal;
        }
    }
}