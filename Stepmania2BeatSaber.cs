using System.Collections;
using System.Collections.Specialized;

namespace Stepmania2BeatSaber
{
    public static class Stepmania2BeatSaber
    {

        private static readonly Dictionary<GameDifficulty, double> pDifficulty = new()
        {
            { GameDifficulty.unknown, 1.0 },
            { GameDifficulty.easy, 0.5 },
            { GameDifficulty.normal, 0.7 },
            { GameDifficulty.hard, 0.9 },
            { GameDifficulty.expert, 1.0 }
        };
        [STAThread]
        static void Main()
        {
            Application.Run(new Stepmania2BeatSaberUI());
            
            
            //Helper.Output("Press any key to exit...", DebugState.on);
            //Console.ReadKey();
        }


        public static OrderedDictionary GetRawNotes(string directory, string fn)
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
                            int beatcount = 0;
                            Helper.Output("//Found Difficulty - " + Helper.GameDifficultyToString(difficulty), ConsoleColor.Yellow, DebugState.on);
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
        public static OrderedDictionary CreatBeatSabreEquivalent(OrderedDictionary allData, double offset, double bpm)
        {
            OrderedDictionary retVal = new();
            foreach (GameDifficulty key in allData.Keys)
            {
                Helper.Output("Creating song: " + Helper.GameDifficultyToString(key), ConsoleColor.Cyan, DebugState.on);
                ArrayList notesByDifficulty = new();
                ArrayList rawBeatsArray = new();
                OrderedDictionary songData = new();
                double baseBeats = 0;
                //Set the base beats + account for offset + scale with difficulty
                baseBeats += offset * (bpm / 60.0);
                //------------------------
                if (allData != null && allData[key] != null)
                { 
                    var temp = allData[key];
                    if (temp != null)
                        notesByDifficulty = (ArrayList)temp;
                }
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
                            var temp = measure[i];
                            if (temp != null)
                            {
                                currentBeat = (RawBeat)temp;
                                if (currentBeat.Count != 4)
                                    throw new NotImplementedException("New Note Count found - expected only 4");
                                //Save the interval -----------------------------
                                currentBeat.BeatTime = baseBeats;
                                rawBeatsArray.Add(currentBeat);
                                baseBeats += interval;
                            }
                        }
                    }
                }
                
                ArrayList retArray = StandardNoteParsing(rawBeatsArray);
                songData.Add("notes", retArray);
                //-----------
                ArrayList obstArray = ObstaclesArray(rawBeatsArray);
                songData.Add("obstacles", obstArray);
                //-----------
                retVal.Add(key, songData);
            }
            return retVal;
        }
        public static ArrayList StandardNoteParsing(ArrayList rawBeatsArray)
        {
            RawBeat previousBeat = new();
            ArrayList retArray = new();
            foreach (RawBeat currentBeat in rawBeatsArray)
            {
                //making this throw an error so I fucking read it... dog damnit morty
                if (previousBeat.Mask.Equals(currentBeat.Mask))
                {
                    //Repeat Pattern - Change it up!
                    RepeatNoteParsing(currentBeat, ref previousBeat, ref retArray);
                }
                else
                {
                    //New Pattern
                    RawNote r;
                    for (int count = 0; count < currentBeat.Count; count++)
                    {
                        r = currentBeat.Get(count);
                        if (r.RawNoteType != RawNoteType.none)
                        {
                            if (r.RawNoteType == RawNoteType.normal)
                            {
                                BSaberNote note = new();
                                if (count > 1)
                                    note.NoteType = NoteType.blue;
                                switch (r.RawDirection)
                                {
                                    case RawDirection.left:
                                        {
                                            note.CutDirection = CutDirection.left;
                                            note.LineIndex = LineIndex.left;
                                            note.LineLayer = LineLayer.middle;
                                            break;
                                        }
                                    case RawDirection.down:
                                        {
                                            if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                            {
                                                note.NoteType = NoteType.blue;
                                                note.LineIndex = LineIndex.centerRight;
                                                note.LineLayer = LineLayer.bottom;
                                            }
                                            else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                            {
                                                note.LineIndex = LineIndex.centerLeft;
                                                note.LineLayer = LineLayer.middle;
                                            }
                                            else
                                            {
                                                note.LineIndex = LineIndex.centerLeft;
                                            }
                                            note.CutDirection = CutDirection.down;
                                            break;
                                        }
                                    case RawDirection.up:
                                        {
                                            if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                            {
                                                note.NoteType = NoteType.red;
                                                note.LineIndex = LineIndex.centerLeft;
                                                note.LineLayer = LineLayer.top;
                                            }
                                            else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                            {
                                                note.LineIndex = LineIndex.centerRight;
                                                note.LineLayer = LineLayer.middle;
                                            }
                                            else
                                            {
                                                note.LineIndex = LineIndex.centerRight;
                                                note.LineLayer = LineLayer.middle;
                                            }
                                            note.CutDirection = CutDirection.up;
                                            break;
                                        }
                                    case RawDirection.right:
                                        {
                                            note.CutDirection = CutDirection.right;
                                            note.LineIndex = LineIndex.right;
                                            note.LineLayer = LineLayer.middle;
                                            break;
                                        }
                                    default:
                                        {
                                            throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                        }
                                }
                                note.Time = currentBeat.BeatTime;
                                retArray.Add(note);
                                if (r.RawNoteType != RawNoteType.normal && r.RawNoteType != RawNoteType.holdStart && r.RawNoteType != RawNoteType.holdEnd)
                                {
                                    Helper.Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta, DebugState.on);
                                }
                            }
                        }
                    }
                    //--- set the new beat
                    if (!currentBeat.Mask.Equals("0000"))
                    {
                        previousBeat = currentBeat;
                    }
                }
            }
            return retArray;
        }
        public static ArrayList ObstaclesArray(ArrayList rawBeatsArray)
        {
            ArrayList obstArray = new();
            if (rawBeatsArray != null)
            {
                BSaberObstacle oLeft = new();
                BSaberObstacle oCenterLeft = new();
                BSaberObstacle oCenterRight = new();
                BSaberObstacle oRight = new();
                BSaberObstacle o = new();
                bool centerOccupied = false;
                foreach (RawBeat b in rawBeatsArray)
                {
                    foreach (RawNote r in b.RawNoteArray)
                    {
                        if ((!centerOccupied && r.RawNoteType == RawNoteType.holdStart) || r.RawNoteType == RawNoteType.holdEnd)
                        {
                            if(r.RawDirection == RawDirection.left) { 
                                o = oLeft;
                                o.LineIndex = LineIndex.left;
                            }
                            else if (r.RawDirection == RawDirection.down)
                            {
                                o = oCenterLeft;
                                o.LineIndex = LineIndex.centerLeft;
                            }
                            else if (r.RawDirection == RawDirection.up)
                            {
                                o = oCenterRight;
                                o.LineIndex = LineIndex.centerRight;
                            }
                            else if (r.RawDirection == RawDirection.right)
                            {
                                o = oRight;
                                o.LineIndex = LineIndex.right;
                            }
                            //-------------
                            if (r.RawNoteType == RawNoteType.holdStart)
                            {
                                if (!o.IsOpen) { 
                                    o.IsOpen = true;
                                    o.Time = b.BeatTime;
                                    if (o.LineIndex == LineIndex.centerLeft || o.LineIndex == LineIndex.centerRight)
                                    {
                                        centerOccupied = true;
                                    }
                                }
                                else
                                    Helper.Output("POSSIBLE ERROR: Trying to open two obstacles at once.", ConsoleColor.Red, DebugState.on);
                            }
                            else if (r.RawNoteType == RawNoteType.holdEnd)
                            {
                                if (o.IsOpen)
                                {
                                    o.IsOpen = false;
                                    if (centerOccupied)
                                        centerOccupied = false;
                                    //-------------
                                    o.Duration = b.BeatTime - o.Time;
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
        public static void RepeatNoteParsing(RawBeat currentBeat, ref RawBeat previousBeat, ref ArrayList retArray)
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
                        note.NoteType = NoteType.blue;
                    //------------------------
                    if (currentBeat.RepeatException != RepeatException.none)
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
                                    note.CutDirection = CutDirection.right;
                                    note.LineIndex = LineIndex.centerLeft;
                                    saveMask[count] = 'o';
                                    note.LineLayer = LineLayer.middle;
                                    break;
                                }
                            case RawDirection.down:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note.NoteType = NoteType.blue;
                                        note.LineIndex = LineIndex.centerRight;
                                        note.LineLayer = LineLayer.bottom;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note.LineIndex = LineIndex.centerLeft;
                                        note.LineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note.LineIndex = LineIndex.centerLeft;
                                        note.LineLayer = LineLayer.middle;
                                    }
                                    note.CutDirection = CutDirection.up;
                                    saveMask[count] = 'x';
                                    break;
                                }
                            case RawDirection.up:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note.NoteType = NoteType.red;
                                        note.LineIndex = LineIndex.centerLeft;
                                        note.LineLayer = LineLayer.middle;
                                    }
                                    else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                    {
                                        note.LineIndex = LineIndex.centerRight;
                                        note.LineLayer = LineLayer.middle;
                                    }
                                    else
                                    {
                                        note.LineIndex = LineIndex.centerRight;
                                        note.LineLayer = LineLayer.top;
                                    }
                                    //----------------
                                    note.CutDirection = CutDirection.down;
                                    note.LineLayer = LineLayer.middle;
                                    saveMask[count] = 'x';
                                    break;
                                }
                            case RawDirection.right:
                                {
                                    note.CutDirection = CutDirection.left;
                                    note.LineIndex = LineIndex.centerRight;
                                    saveMask[count] = 'o';
                                    note.LineLayer = LineLayer.middle;
                                    break;
                                }
                            default:
                                {
                                    throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                }
                        }
                    }
                    note.Time = currentBeat.BeatTime;
                    retArray.Add(note);
                    if (r.RawNoteType != RawNoteType.normal && r.RawNoteType != RawNoteType.holdStart && r.RawNoteType != RawNoteType.holdEnd)
                    {
                        Helper.Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta, DebugState.on);
                    }
                }
            }
            //--- set the new beat
            if (!currentBeat.Mask.Equals("0000"))
            {
                currentBeat.Mask = new String(saveMask);
                previousBeat = currentBeat;
            }
        }
        public static void RepeatExceptionsHandle_DoubleOut(ref RawNote r, ref BSaberNote note, ref char[] saveMask, ref int count)
        {
            switch (r.RawDirection)
            {
                case RawDirection.left:
                    {
                        note.CutDirection = CutDirection.down;
                        note.LineIndex = LineIndex.centerLeft;
                        saveMask[count] = 'o';
                        note.LineLayer = LineLayer.bottom;
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
                        note.CutDirection = CutDirection.down;
                        note.LineIndex = LineIndex.centerRight;
                        saveMask[count] = 'o';
                        note.LineLayer = LineLayer.bottom;
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Too many notes. Something is critically wrong.");
                    }
            }
        }
        public static void RepeatExceptionsHandle_UpDown(ref RawNote r, ref BSaberNote note, ref char[] saveMask, ref int count)
        {
            switch (r.RawDirection)
            {
                case RawDirection.left:
                    {
                        throw new NotSupportedException("Shouldn't have this case.");
                    }
                case RawDirection.down:
                    {
                        note.LineIndex = LineIndex.centerLeft;
                        note.CutDirection = CutDirection.up;
                        saveMask[count] = 'x';
                        break;
                    }
                case RawDirection.up:
                    {
                        note.LineIndex = LineIndex.centerRight;
                        note.CutDirection = CutDirection.down;
                        note.LineLayer = LineLayer.middle;
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
    }
    public enum GameDifficulty
    {
        unknown,
        easy,
        normal,
        hard,
        expert
    }
}