using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum LineIndex{
        left,
        centerLeft,
        centerRight,
        right
    }

    public enum LineLayer{
        bottom,
        middle,
        top
    }

    public enum Type{
        red,
        blue
    }

    public enum CutDirection{
        up,
        down,
        left,
        right,
        upleft,
        upright,
        downleft,
        downright
    }

    public class Note{
        public Note(){
            Time = 0;
            LineIndex = LineIndex.left;
            LineLayer = LineLayer.bottom;
            Type = Type.red;
            CutDirection = CutDirection.left;
        }
        public double Time { get; set; }
        public LineIndex LineIndex { get; set; }
        public LineLayer LineLayer { get; set; }
        public Type Type { get; set; }
        public CutDirection CutDirection { get; set; }
        public JObject ToJOject(){
            JObject retVal = new(new JProperty("_time", Time),
                new JProperty("_lineIndex", LineIndex),
                new JProperty("_lineLayer", LineLayer),
                new JProperty("_type", Type),
                new JProperty("_cutDirection", CutDirection));
            return retVal;
        }
    }
}