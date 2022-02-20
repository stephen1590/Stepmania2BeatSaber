using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepmania2BeatSaber
{
    public class BeatSaberEquivalent
    {
        public int smid { get; set; } = 0;
        public List<List<BeatSaberEquivalentNotes>> bs { get; set; } = new();
    }
    public class BeatSaberEquivalentNotes
    {
        public int lineLayer { get; set; } = 0;
        public string type { get; set; } = "";
        public string cutDirection { get; set; } = "";
        public int lineIndex { get; set; } = 0;
    }
    public class TempBSaberEquivalentHelper
    {
        private static readonly List<BeatSaberEquivalent> bs = Helper.getBeatEquivalents();
        public TempBSaberEquivalentHelper()
        {

        }
        public Dictionary<int, List<BSaberNote>> getBSaberReplacementNotes(Dictionary<int, FoundPattern> foundPatterns)
        {
            Dictionary<int, List<BSaberNote>> retVal = new();
            List<BSaberNote> notes = new();

            List<BSaberNote> l = new();
            bool found = false;
            BeatSaberEquivalent temp = new();
            foreach(FoundPattern f in foundPatterns.Values)
            {
                foreach (BeatSaberEquivalent e in bs)
                {
                    if (e.smid == f.patternId)
                    {
                        temp = e;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Helper.Output("Current Pattern case is not programmed: " + f.patternId.ToString());
                }
                int beatIndex = 0;
                for(int i = 0; i < temp.bs.Count; i++)
                {
                    int bIndex = i + f.startIndex;
                    if (bIndex >= temp.bs.Count)
                        bIndex = bIndex - temp.bs.Count;
                    List<BeatSaberEquivalentNotes> beat = temp.bs[bIndex];

                    notes = new();
                    foreach(BeatSaberEquivalentNotes bsn in beat)
                    {
                        BSaberNote n = new();

                        n._lineLayer = (LineLayer)bsn.lineLayer;
                        n._lineIndex = (LineIndex)bsn.lineIndex;

                        if (bsn.type == "red")
                            n._type = NoteType.red;
                        else
                            n._type = NoteType.blue;

                        if (bsn.cutDirection.ToLower() == "up")
                            n._cutDirection = CutDirection.up;
                        else if(bsn.cutDirection.ToLower() == "down")
                            n._cutDirection = CutDirection.down;
                        else if(bsn.cutDirection.ToLower() == "left")
                            n._cutDirection = CutDirection.left;
                        else if(bsn.cutDirection.ToLower() == "right")
                            n._cutDirection = CutDirection.right;
                        else if(bsn.cutDirection.ToLower() == "upleft")
                            n._cutDirection = CutDirection.upleft;
                        else if(bsn.cutDirection.ToLower() == "upright")
                            n._cutDirection = CutDirection.upright;
                        else if(bsn.cutDirection.ToLower() == "downleft")
                            n._cutDirection = CutDirection.downleft;
                        else if(bsn.cutDirection.ToLower() == "downright")
                            n._cutDirection = CutDirection.downright;

                        if(beatIndex <= f.rawBeatIndex.Count)
                        {
                            n._beatIndex = f.rawBeatIndex[beatIndex];
                            //Something to consider - buffer all the beats in the sequence - if we have an overlap and the note sequence is the same, keep going - we might be extending the pattern
                            notes.Add(n);
                        }
                        else
                        {
                            throw new NotSupportedException("Beat Index Mismatch!");
                        }
                    }
                    retVal.Add(f.rawBeatIndex[beatIndex], notes);
                    beatIndex++;
                }
            }
            return retVal;
        }
    }
}
