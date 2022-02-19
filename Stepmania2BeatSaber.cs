using System.Collections;
using System.Collections.Specialized;

namespace Stepmania2BeatSaber
{
    internal class Stepmania2BeatSaber
    {
        [STAThread]
        public static void Main()
        {
            //Run the UI
            Application.Run(new UserInterface());
        }
    }
    public class SM2BS
    {
        public Options o = new();
        private readonly List<GameDifficulty> GameDifficultyKeys = new()
        {
            GameDifficulty.Easy,
            GameDifficulty.Normal,
            GameDifficulty.Hard,
            GameDifficulty.Expert,
            GameDifficulty.ExpertPlus
        };
        public SM2BS()
        {
            Helper.optionsPopulate(ref o);
        }
        private List<GameDifficulty> DifficultiesFromo()
        {
            List<GameDifficulty> difficulties = GameDifficultyKeys;
            if (o.MyGameDifficulty != GameDifficulty.All)
            {
                difficulties = new List<GameDifficulty> { o.MyGameDifficulty };
            }
            return difficulties;
        }
        public OrderedDictionary GetRawNotes(string directory, string fn)
        {
            Helper.Output("Reading data...", ConsoleColor.Cyan, DebugState.on);
            OrderedDictionary playCollection = new();
            OrderedDictionary retHash = new();
            string line;
            string filename = directory + @"\" + fn;
            try
            {
                using (StreamReader reader = new(filename))
                {
                    // Read one line from file
                    Int32 foundItems = 0;
                    while (reader != null && !reader.EndOfStream && !foundItems.Equals(2))
                    {
                        line = Helper.GetNextLine(reader);
                        if (line != null && line.StartsWith("#OFFSET"))
                        {
                            line = line[8..^1];
                            retHash["offset"] = Double.Parse(line.Trim()) * -1;
                            foundItems++;
                        }
                        else if (line != null && line.StartsWith("#BPMS"))
                        {
                            line = line[12..^1];
                            retHash.Add("bpm", Double.Parse(line.Trim()));
                            foundItems++;
                        }
                    }
                    while (reader != null && !reader.EndOfStream)
                    {
                        line = Helper.GetNextLine(reader);
                        if (line != null && line.StartsWith("//") && line.IndexOf("dance-single") > -1)
                        {
                            reader.ReadLine();
                            reader.ReadLine();
                            reader.ReadLine();
                            // Get Difficulty
                            GameDifficulty difficulty = Helper.FindDifficulty(Helper.GetNextLine(reader));
                            List<GameDifficulty> difficulties = DifficultiesFromo();
                            if (difficulties.Contains(difficulty))
                            {
                                int beatcount = 0;
                                Helper.Output("//Found Difficulty - " + difficulty.ToString(), ConsoleColor.Yellow, DebugState.on);
                                reader.ReadLine();
                                reader.ReadLine();
                                ArrayList notesByDifficulty = new();
                                while (!reader.EndOfStream)
                                {
                                    bool newDifficulty = false;
                                    // Get Noteset
                                    ArrayList measure = new();
                                    while (!reader.EndOfStream)
                                    {
                                        line = Helper.GetNextLine(reader);
                                        if (line != null)
                                        {
                                            if (!line.StartsWith(",") && !line.StartsWith(";"))
                                            {
                                                RawBeat currentBeat = new();
                                                for (Int32 i = 0; i < line.Length; i++)
                                                {
                                                    RawNote rawNote = new(line, i);
                                                    currentBeat.Add(rawNote);
                                                }
                                                measure.Add(currentBeat);
                                                beatcount += 1;
                                                Helper.Output("Note#" + beatcount.ToString("D3") + ":  " + line, ConsoleColor.Yellow);
                                            }
                                            else
                                            {
                                                if (line.StartsWith(";"))
                                                    newDifficulty = true;
                                                break;
                                            }
                                        }
                                    }
                                    notesByDifficulty.Add(measure);
                                    if (newDifficulty)
                                        break;
                                }
                                playCollection.Add(difficulty, notesByDifficulty);
                            }
                            else
                            {
                                Helper.Output("//Skipping Difficulty - " + difficulty.ToString() + " - as part of config.", ConsoleColor.Yellow, DebugState.on);
                            }
                        }
                    }
                }
                retHash.Add("songs", playCollection);
            }
            catch (Exception ex)
            {
                Helper.Output("Error reading file. " + Environment.NewLine + ex.Message, ConsoleColor.Red, DebugState.on);
            }
            return retHash;
        }
        public OrderedDictionary CreatBeatSabreEquivalent(OrderedDictionary allData, double offset, double bpm)
        {
            OrderedDictionary retVal = new();
            List<GameDifficulty> difficulties = DifficultiesFromo();
            foreach (GameDifficulty key in difficulties)
            {
                //------------------------
                if (allData != null && allData.Contains(key))
                {
                    Helper.Output("Creating song: " + ((GameDifficulty)key).ToString(), ConsoleColor.Cyan, DebugState.on);
                    ArrayList notesByDifficulty = new();
                    List<RawBeat> rawBeats = new();
                    OrderedDictionary songData = new();
                    double baseBeats = 0;
                    //Set the base beats + account for offset + scale with difficulty
                    baseBeats += offset * (bpm / 60.0);
                    //------------------------
                    var temp = allData[key];
                    if (temp != null)
                        notesByDifficulty = (ArrayList)temp;
                    //Remove the measures and calculate the interval
                    if (notesByDifficulty != null)
                    {
                        foreach (ArrayList measure in notesByDifficulty)
                        {
                            // Default is 4 notes per currentBeat - we may have more, so calculate the interval
                            double interval = 4.0 / (double)measure.Count;
                            RawBeat currentBeat = new();
                            // split the measure into invididual notes/beats
                            for (int i = 0; i < measure.Count; i++)
                            {
                                temp = measure[i];
                                if (temp != null)
                                {
                                    currentBeat = (RawBeat)temp;
                                    if (currentBeat.Count != 4)
                                        throw new NotImplementedException("New Note Count found - expected only 4");
                                    //Save the interval -----------------------------
                                    currentBeat.BeatTime = baseBeats;
                                    rawBeats.Add(currentBeat);
                                    baseBeats += interval;
                                }
                            }
                        }
                    }
                    /*===============================================
                     * Create BeatSaber equivalents and fix it up given specific options!
                     *===============================================*/
                    List<BSaberBeat> tbp = TranslatedBeatParsing(ref rawBeats);
                    List<BSaberBeat> bSaberBeats = StandardBeatParsing(rawBeats);
                    if (o.ResolveRepeats)
                    {
                        CheckRepeats(ref bSaberBeats);
                    }
                    if (o.ResolveConflicts)
                    {
                        CheckConflicts(ref bSaberBeats);
                    }
                    /*===============================================
                     * We finished corrections with notes - collapse the beats into notes
                     *===============================================*/
                    //collapse the notes
                    List<BSaberNote> bSaberNotes = new();
                    foreach (BSaberBeat b in bSaberBeats)
                    {
                        foreach (BSaberNote n in b.getNotes())
                        {
                            bSaberNotes.Add(n);
                        }
                    }
                    songData.Add("notes", bSaberNotes);
                    /*===============================================
                     * Parse out the obstacles
                     *===============================================*/
                    //Obstacles are options! check the options.
                    List<BSaberObstacle> obstacles = new();
                    if (o.ApplyObstacles)
                    {
                        obstacles = ObstaclesParse(rawBeats);
                    }
                    songData.Add("obstacles", obstacles);
                    /* ===============================================
                     * We're done!
                     * ===============================================*/
                    retVal.Add(key, songData);
                }
            }
            return retVal;
        }
        public static List<BSaberBeat> LegacyBeatParsing(List<RawBeat> rawBeats)
        {
            List<BSaberBeat> retArray = new();
            int beatIndex = 1;
            foreach (RawBeat currentBeat in rawBeats)
            {
                //New Pattern
                BSaberBeat b = new();
                RawNote r;
                for (int count = 0; count < currentBeat.Count; count++)
                {
                    r = currentBeat.Get(count);
                    if (r.RawNoteType != RawNoteType.none && r.RawNoteType == RawNoteType.normal)
                    {
                        BSaberNote note = new();
                        if (count > 1)
                            note._type = NoteType.blue;
                        switch (r.RawDirection)
                        {
                            case RawDirection.left:
                                {
                                    note._cutDirection = CutDirection.left;
                                    note._lineIndex = LineIndex.left;
                                    note._lineLayer = LineLayer.middle;
                                    break;
                                }
                            case RawDirection.down:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note._type = NoteType.blue;
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.bottom;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note._lineIndex = LineIndex.centerLeft;
                                    }
                                    note._cutDirection = CutDirection.down;
                                    break;
                                }
                            case RawDirection.up:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note._type = NoteType.red;
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._lineLayer = LineLayer.top;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    note._cutDirection = CutDirection.up;
                                    break;
                                }
                            case RawDirection.right:
                                {
                                    note._cutDirection = CutDirection.right;
                                    note._lineIndex = LineIndex.right;
                                    note._lineLayer = LineLayer.middle;
                                    break;
                                }
                            default:
                                {
                                    throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                }
                        }
                        note._time = currentBeat.BeatTime;
                        note._beatIndex = beatIndex;
                        b.addNote(note);
                        CheckNoteException(r);
                    }
                }
                if (b.Count() > 0)
                {
                    retArray.Add(b);
                }
                beatIndex++;
            }
            return retArray;
        }
        public List<BSaberBeat> StandardBeatParsing(List<RawBeat> rawBeats)
        {
            List<BSaberBeat> retArray = new();
            int beatIndex = 1;
                List<RawBeat> rawBeatBuffer = new();
                foreach (RawBeat currentBeat in rawBeatBuffer)
                {
                    //New Pattern
                    BSaberBeat b = new();
                    RawNote r;
                    for (int count = 0; count < currentBeat.Count; count++)
                    {
                        r = currentBeat.Get(count);
                        if (r.RawNoteType != RawNoteType.none && r.RawNoteType == RawNoteType.normal)
                        {
                            BSaberNote note = new();
                            if (count > 1)
                                note._type = NoteType.blue;
                            switch (r.RawDirection)
                            {
                                case RawDirection.left:
                                    {
                                        note._cutDirection = CutDirection.left;
                                        note._lineIndex = LineIndex.left;
                                        break;
                                    }
                                case RawDirection.down:
                                    {
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._cutDirection = CutDirection.down;
                                        break;
                                    }
                                case RawDirection.up:
                                    {
                                        note._lineIndex = LineIndex.centerRight;
                                        note._cutDirection = CutDirection.up;
                                        break;
                                    }
                                case RawDirection.right:
                                    {
                                        note._cutDirection = CutDirection.right;
                                        note._lineIndex = LineIndex.right;
                                        break;
                                    }
                                default:
                                    {
                                        throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                    }
                            }
                            note._time = currentBeat.BeatTime;
                            note._beatIndex = beatIndex;
                            b.addNote(note);
                            CheckNoteException(r);
                        }
                    }
                    if (b.Count() > 0)
                    {
                        retArray.Add(b);
                    }
                    beatIndex++;
                }
            return retArray;
        }
        public static List<BSaberObstacle> ObstaclesParse(List<RawBeat> rawBeats)
        {
            List<BSaberObstacle> obstArray = new();
            if (rawBeats != null)
            {
                BSaberObstacle oLeft = new();
                BSaberObstacle oCenterLeft = new();
                BSaberObstacle oCenterRight = new();
                BSaberObstacle oRight = new();
                BSaberObstacle o = new();
                bool centerOccupied = false;
                foreach (RawBeat b in rawBeats)
                {
                    foreach (RawNote r in b.RawNoteArray)
                    {
                        if ((!centerOccupied && r.RawNoteType == RawNoteType.holdStart) || r.RawNoteType == RawNoteType.holdEnd)
                        {
                            if(r.RawDirection == RawDirection.left) { 
                                o = oLeft;
                                o._lineIndex = LineIndex.left;
                            }
                            else if (r.RawDirection == RawDirection.down)
                            {
                                o = oCenterLeft;
                                o._lineIndex = LineIndex.centerLeft;
                            }
                            else if (r.RawDirection == RawDirection.up)
                            {
                                o = oCenterRight;
                                o._lineIndex = LineIndex.centerRight;
                            }
                            else if (r.RawDirection == RawDirection.right)
                            {
                                o = oRight;
                                o._lineIndex = LineIndex.right;
                            }
                            //-------------
                            if (r.RawNoteType == RawNoteType.holdStart)
                            {
                                if (!o._isOpen) { 
                                    o._isOpen = true;
                                    o._time = b.BeatTime;
                                    if (o._lineIndex == LineIndex.centerLeft || o._lineIndex == LineIndex.centerRight)
                                    {
                                        centerOccupied = true;
                                    }
                                }
                                else
                                    Helper.Output("POSSIBLE ERROR: Trying to open two obstacles at once.", ConsoleColor.Red, DebugState.on);
                            }
                            else if (r.RawNoteType == RawNoteType.holdEnd)
                            {
                                if (o._isOpen)
                                {
                                    o._isOpen = false;
                                    if (centerOccupied)
                                        centerOccupied = false;
                                    //-------------
                                    o._duration = b.BeatTime - o._time;
                                    obstArray.Add(o);
                                    o = new();
                                    //-------------
                                    if (r.RawDirection == RawDirection.left)
                                        oLeft = new();
                                    else if (r.RawDirection == RawDirection.down)
                                        oCenterLeft = new();
                                    else if (r.RawDirection == RawDirection.up)
                                        oCenterRight = new();
                                    else if (r.RawDirection == RawDirection.right)
                                        o = oRight = new();
                                }
                                else
                                    Helper.Output("POSSIBLE ERROR: Trying to close something that wasn't opened.", ConsoleColor.Red, DebugState.on);
                            }
                            
                        }
                    }
                }
            }
            return obstArray;
        }
        private List<BSaberBeat> TranslatedBeatParsing(ref List<RawBeat> rawBeats)
        {
            System.DateTime startTime = System.DateTime.Now;
            List<BSaberBeat> retArray = new();
            if (o.TranslatePatterns)
            {
                int beatIndex = 1;
                List<RawBeat> rawBeatBuffer = new();
                List<Pattern> patterns = Helper.getPatterns();
                foreach (Pattern p in patterns)
                {
                    for (int m = 0; m<p.mask.Count; m++)
                    {
                        if (m > 0 && p.name.IndexOf("Double") >-1)
                        {
                            //Double patterns are repeats that we can skip on the 2nd iteration
                            continue;
                        }
                        List<string> currentMask = new();
                        while(currentMask.Count != p.mask.Count)
                        {
                            int mIndex = m + currentMask.Count;
                            if(mIndex >= p.mask.Count)
                            {
                                mIndex = (m + currentMask.Count) - p.mask.Count;
                            }
                            currentMask.Add(p.mask[mIndex]);
                        }
                        for (int index = 0; index < rawBeats.Count; index++)
                        {
                            if (rawBeats[index].Mask == currentMask[0])
                            {
                                int startIndex = index;
                                int localIndex = 1;
                                while (localIndex < currentMask.Count)
                                {
                                    //Keep going long as we have enough beats
                                    if ((index + localIndex) < rawBeats.Count)
                                    {
                                        RawBeat r = rawBeats[index + localIndex];
                                        //skip any empty beats
                                        if (r.Mask.Equals("0000"))
                                        {
                                            //Get the next beat
                                            index++;
                                        }
                                        else
                                        {
                                            //Keep going unless the masks finally differ
                                            if (r.Mask != currentMask[localIndex])
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                //we had a match, so let's check the next one!
                                                localIndex++;
                                                index++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                if (localIndex == currentMask.Count)
                                {
                                    //If we exhausted our local index, we have a matching pattern
                                    Helper.Output("=================================", DebugState.on);
                                    Helper.Output(String.Format("Pattern [{0}].[{1}]{2}Found at beat index: [{3}]", p.name, m.ToString(),"\n", startIndex.ToString()),DebugState.on);
                                }
                            }
                        }
                    }
                }
                System.DateTime endTime = System.DateTime.Now;
                System.TimeSpan diff = endTime - startTime;
                Helper.Output("=================================", DebugState.on);
                Helper.Output(String.Format("Sequencing took: {0}ms",diff.TotalMilliseconds.ToString()), DebugState.on);
                Helper.Output("=================================", DebugState.on);
                foreach (RawBeat currentBeat in rawBeatBuffer)
                    {
                        //New Pattern
                        BSaberBeat b = new();
                        RawNote r;
                        for (int count = 0; count < currentBeat.Count; count++)
                        {
                            r = currentBeat.Get(count);
                            if (r.RawNoteType != RawNoteType.none && r.RawNoteType == RawNoteType.normal)
                            {
                                BSaberNote note = new();
                                if (count > 1)
                                    note._type = NoteType.blue;
                                switch (r.RawDirection)
                                {
                                    case RawDirection.left:
                                        {
                                            note._cutDirection = CutDirection.left;
                                            note._lineIndex = LineIndex.left;
                                            break;
                                        }
                                    case RawDirection.down:
                                        {
                                            note._lineIndex = LineIndex.centerLeft;
                                            note._cutDirection = CutDirection.down;
                                            break;
                                        }
                                    case RawDirection.up:
                                        {
                                            note._lineIndex = LineIndex.centerRight;
                                            note._cutDirection = CutDirection.up;
                                            break;
                                        }
                                    case RawDirection.right:
                                        {
                                            note._cutDirection = CutDirection.right;
                                            note._lineIndex = LineIndex.right;
                                            break;
                                        }
                                    default:
                                        {
                                            throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                        }
                                }
                                note._time = currentBeat.BeatTime;
                                note._beatIndex = beatIndex;
                                b.addNote(note);
                                CheckNoteException(r);
                            }
                        }
                        if (b.Count() > 0)
                        {
                            retArray.Add(b);
                        }
                        beatIndex++;
                    }
            }
            return retArray;
        }
        public void CheckConflicts(ref List<BSaberBeat> bSaberBeats)
        {
        }
        private void CheckRepeats(ref List<BSaberBeat> bSaberBeats)
        {
        }
        private void CheckRepeats_old(RawBeat currentBeat, ref RawBeat previousBeat, ref List<BSaberNote> retArray)
        {
            char[] saveMask = currentBeat.Mask.ToCharArray();
            //New Pattern
            RawNote r;
            for (int count = 0; count < currentBeat.Count; count++)
            {
                r = currentBeat.Get(count);
                if (r.RawNoteType != RawNoteType.none)
                {
                    BSaberNote note = new();
                    if (count > 1)
                        note._type = NoteType.blue;
                    //------------------------
                    if (o.ResolveConflicts && currentBeat.RepeatException != RepeatException.none)
                    {
                        if (currentBeat.RepeatException == RepeatException.doubleOut)
                        {
                            RepeatExceptionsHandle_DoubleOut(ref r, ref note, ref saveMask, ref count);
                        }
                        else if (currentBeat.RepeatException == RepeatException.upDown)
                        {
                            RepeatExceptionsHandle_UpDown(ref r, ref note, ref saveMask, ref count);
                        }
                    }
                    else { 
                        switch (r.RawDirection)
                        {
                            case RawDirection.left:
                                {
                                    note._cutDirection = CutDirection.right;
                                    note._lineIndex = LineIndex.centerLeft;
                                    saveMask[count] = 'o';
                                    note._lineLayer = LineLayer.middle;
                                    break;
                                }
                            case RawDirection.down:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note._type = NoteType.blue;
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.bottom;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    note._cutDirection = CutDirection.up;
                                    saveMask[count] = 'x';
                                    break;
                                }
                            case RawDirection.up:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note._type = NoteType.red;
                                        note._lineIndex = LineIndex.centerLeft;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note._lineIndex = LineIndex.centerRight;
                                        note._lineLayer = LineLayer.top;
                                    }
                                    //----------------
                                    note._cutDirection = CutDirection.down;
                                    note._lineLayer = LineLayer.middle;
                                    saveMask[count] = 'x';
                                    break;
                                }
                            case RawDirection.right:
                                {
                                    note._cutDirection = CutDirection.left;
                                    note._lineIndex = LineIndex.centerRight;
                                    saveMask[count] = 'o';
                                    note._lineLayer = LineLayer.middle;
                                    break;
                                }
                            default:
                                {
                                    throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                }
                        }
                    }
                    note._time = currentBeat.BeatTime;
                    retArray.Add(note);
                    CheckNoteException(r);
                }
            }
            //--- set the new beat
            if (!currentBeat.Mask.Equals("0000"))
            {
                currentBeat.Mask = new String(saveMask);
                previousBeat = currentBeat;
            }
        }
        private static void RepeatExceptionsHandle_DoubleOut(ref RawNote r, ref BSaberNote note, ref char[] saveMask, ref int count)
        {
            switch (r.RawDirection)
            {
                case RawDirection.left:
                    {
                        note._cutDirection = CutDirection.down;
                        note._lineIndex = LineIndex.centerLeft;
                        saveMask[count] = 'o';
                        note._lineLayer = LineLayer.bottom;
                        break;
                    }
                case RawDirection.down:
                    {
                        throw new NotSupportedException("Shouldn't have this case.");
                    }
                case RawDirection.up:
                    {
                        throw new NotSupportedException("Shouldn't have this case.");
                    }
                case RawDirection.right:
                    {
                        note._cutDirection = CutDirection.down;
                        note._lineIndex = LineIndex.centerRight;
                        saveMask[count] = 'o';
                        note._lineLayer = LineLayer.bottom;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Too many notes. Something is critically wrong.");
                    }
            }
        }
        private static void RepeatExceptionsHandle_UpDown(ref RawNote r, ref BSaberNote note, ref char[] saveMask, ref int count)
        {
            switch (r.RawDirection)
            {
                case RawDirection.left:
                    {
                        throw new NotSupportedException("Shouldn't have this case.");
                    }
                case RawDirection.down:
                    {
                        note._lineIndex = LineIndex.centerLeft;
                        note._cutDirection = CutDirection.up;
                        saveMask[count] = 'x';
                        break;
                    }
                case RawDirection.up:
                    {
                        note._lineIndex = LineIndex.centerRight;
                        note._cutDirection = CutDirection.down;
                        note._lineLayer = LineLayer.middle;
                        saveMask[count] = 'x';
                        break;
                    }
                case RawDirection.right:
                    {
                        throw new NotSupportedException("Shouldn't have this case.");
                    }
                default:
                    {
                        throw new NotImplementedException("Too many notes. Something is critically wrong.");
                    }
            }
        }
        private static void CheckNoteException(RawNote r)
        {
            if (r.RawNoteType != RawNoteType.normal && r.RawNoteType != RawNoteType.holdStart && r.RawNoteType != RawNoteType.holdEnd)
            {
                Helper.Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta, DebugState.on);
            }
        }
    }
}