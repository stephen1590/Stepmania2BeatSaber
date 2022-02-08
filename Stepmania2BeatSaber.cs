using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    static class Stepmania2BeatSaber{
        //private static readonly string pDir = @"C:\src\BeatSaber\BREAK DOWN!\original";
        //private static readonly string pFilename = "BREAK DOWN!.sm";
        private static readonly string pDir = @"C:\src\BeatSaber\Midnite Blaze";
        private static readonly string pFilename = "Midnite Blaze.sm";
        private static readonly string pSongName = pFilename.Split(".")[0];
        private static readonly Dictionary<GameDifficulty, double> pDifficulty = new()
        {
            { GameDifficulty.unknown, 1.0},
            { GameDifficulty.easy, 0.5 },
            { GameDifficulty.normal, 0.7 },
            { GameDifficulty.hard, 0.9 },
            { GameDifficulty.expert, 1.0 }
        };
        public static void Main(){
            double bpm = 0.0;
            double offset = 0.0;
            OrderedDictionary rawDAta = GetRawNotes(pDir, pFilename);
            if (rawDAta != null && rawDAta.Keys.Count > 0){
                var temp = rawDAta["offset"];
                if (temp != null){
                    offset = (double)temp;
                    Helper.Output("Offset Found: " + offset.ToString(), ConsoleColor.Green);
                }
                temp = rawDAta["bpm"];
                if (temp != null)
                {
                    bpm = (double)temp;
                    Helper.Output("BPM Found: " + bpm.ToString(), ConsoleColor.Green);
                }
                temp = rawDAta["songs"];
                if (temp != null){
                    var songs = CreatBeatSabreEquivalent((OrderedDictionary)temp, offset, bpm);
                    Helper.WriteFile(songs, pDir, pSongName);
                }
            }
            Helper.Output("Press any key to exit...");
            Console.ReadKey();
        }


        public static OrderedDictionary GetRawNotes(string directory, string fn){
            Helper.Output("Reading data...", ConsoleColor.Cyan);
            OrderedDictionary playCollection = new();
            OrderedDictionary retHash = new();
            string line;
            string filename = directory + @"\" + fn;
            try{
                using (StreamReader reader = new(filename)){
                    // Read one line from file
                    Int32 foundItems = 0;
                    while (reader != null && !reader.EndOfStream && !foundItems.Equals(2))
                    {
                        line = Helper.GetNextLine(reader);
                        if (line != null && line.StartsWith("#OFFSET")){
                            line = line[8..^1];
                            retHash["offset"] = Double.Parse(line.Trim()) * -1;
                            foundItems++;   
                        }
                        else if (line != null && line.StartsWith("#BPMS")){
                            line = line[12..^1];
                            retHash.Add("bpm", Double.Parse(line.Trim()));
                            foundItems++;
                        }
                    }
                    while (reader != null && !reader.EndOfStream){
                        line = Helper.GetNextLine(reader);
                        if (line != null && line.StartsWith("//") && line.IndexOf("dance-single") > -1){
                            reader.ReadLine();
                            reader.ReadLine();
                            reader.ReadLine();
                            // Get Difficulty
                            GameDifficulty difficulty = FindDifficulty(Helper.GetNextLine(reader));
                            int beatcount = 0;
                            Helper.Output("//Found Difficulty - " + Helper.GameDifficultyToString(difficulty) + " ---------------------------------", ConsoleColor.Yellow);
                            reader.ReadLine();
                            reader.ReadLine();
                            ArrayList notesByDifficulty = new();
                            while (!reader.EndOfStream){
                                var newDifficulty = false;
                                // Get Noteset
                                ArrayList measure = new();
                                while (!reader.EndOfStream){
                                    line = Helper.GetNextLine(reader);
                                    if (line != null){
                                        if (!line.StartsWith(",") && !line.StartsWith(";")){
                                            RawBeat currentBeat = new ();
                                            for (Int32 i = 0; i < line.Length; i++)
                                            {
                                                RawNote rawNote = new(line, i);
                                                currentBeat.Add(rawNote);
                                            }
                                            measure.Add(currentBeat);
                                            beatcount += 1;
                                            Helper.Output("Note#" + beatcount.ToString("D3") + ":  " + line, ConsoleColor.Yellow);
                                        }
                                        else{
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
            catch (Exception ex){
                Helper.Output("Error reading file. " + Environment.NewLine + ex.Message, ConsoleColor.Red);
            }
            return retHash;
        }
        public static OrderedDictionary CreatBeatSabreEquivalent(OrderedDictionary allData, double offset, double bpm)
        {
            OrderedDictionary retVal = new();
            foreach (GameDifficulty key in allData.Keys){
                Helper.Output("Creating song: " + Helper.GameDifficultyToString(key), ConsoleColor.Cyan);
                ArrayList notesByDifficulty = new();
                if (allData != null && allData[key] != null)
                    try{
                        var temp = allData[key];
                        if (temp != null)
                            notesByDifficulty = (ArrayList)temp;
                    }
                    catch{
                        notesByDifficulty = new();
                    }
                ArrayList retArray = new();
                ArrayList obstArray = new();
                double baseBeats = 0;
                //Set the base beats + account for offset + scale with difficulty
                baseBeats += offset * (bpm / 60.0);
                if (notesByDifficulty != null){
                    RawBeat previousBeat = new();
                    foreach (ArrayList measure in notesByDifficulty){
                        // Default is 4 notes per currentBeat - we may have more, so calculate the interval
                        double interval = 4.0 / (double)measure.Count;
                        RawBeat currentBeat = new();
                        for (int i = 0; i < measure.Count; i++)
                        {
                            var temp = measure[i];
                            if (temp != null)
                            {
                                currentBeat = (RawBeat)temp;
                                if (currentBeat.Count != 4)
                                    throw new NotImplementedException("New Note Count found - expected only 4");
                                //-----------------------------
                                // split the measure into invididual notes/beats
                                StandardNoteArray(currentBeat, ref previousBeat, ref baseBeats, ref retArray);
                                baseBeats += interval;
                            }
                        }
                    }
                    retVal.Add(key, retArray);
                }
            }
            return retVal;
        }
        public static void StandardNoteArray(RawBeat currentBeat, ref RawBeat previousBeat, ref double baseBeats, ref ArrayList retArray){
            if (currentBeat != null && previousBeat != null)
            {
                //making this throw an error so I fucking read it... dog damnit morty
                if (previousBeat.Mask.Equals(currentBeat.Mask))
                {
                    //Repeat Pattern - Change it up!
                    RepeatNoteArray(currentBeat, ref previousBeat, ref baseBeats, ref retArray);
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
                                    note.Type = Type.blue;
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
                                                note.Type = Type.blue;
                                                note.LineIndex = LineIndex.centerRight;
                                                note.LineLayer = LineLayer.middle;
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
                                                note.Type = Type.red;
                                                note.LineIndex = LineIndex.centerLeft;
                                            }
                                            else if (currentBeat.ConflictType == ConflictType.verticalSplit)
                                            {
                                                note.LineIndex = LineIndex.centerRight;
                                            }
                                            else
                                            {
                                                note.LineIndex = LineIndex.centerRight;
                                            }
                                            note.LineLayer = LineLayer.middle;
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
                                note.Time = baseBeats;
                                retArray.Add(note);
                                if (r.RawNoteType != RawNoteType.normal)
                                {
                                    Helper.Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta);
                                }
                            }
                            else if (r.RawNoteType == RawNoteType.holdStart || r.RawNoteType == RawNoteType.holdEnd)
                            {
                                ObstaclesAsNoteArray(currentBeat, ref baseBeats, ref retArray);
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
        }
        public static void ObstaclesAsNoteArray(RawBeat currentBeat, ref double baseBeats, ref ArrayList retArray)
        {
            if (currentBeat != null)
            {
            }
        }
        public static void RepeatNoteArray(RawBeat currentBeat, ref RawBeat previousBeat, ref double baseBeats, ref ArrayList retArray)
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
                            note.Type = Type.blue;
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
                                        note.Type = Type.blue;
                                        note.LineIndex = LineIndex.centerRight;
                                        note.LineLayer = LineLayer.middle;
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
                                    note.CutDirection = CutDirection.up;
                                    note.LineLayer = LineLayer.middle;
                                    saveMask[count] = 'x';
                                    break;
                                }
                            case RawDirection.up:
                                {
                                    if (currentBeat.ConflictType == ConflictType.doubleHandConflict)
                                    {
                                        note.Type = Type.red;
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
                        note.Time = baseBeats;
                        retArray.Add(note);
                        if (r.RawNoteType != RawNoteType.normal)
                        {
                            Helper.Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta);
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
        public static GameDifficulty FindDifficulty(string difficulty){
            switch (difficulty.Split(":")[0].Trim()){
                case "Beginner":{
                        return GameDifficulty.easy;
                    }
                case "Easy":{
                        return GameDifficulty.normal;
                    }
                case "Medium":{
                        return GameDifficulty.hard;
                    }
                case "Hard":{
                        return GameDifficulty.expert;
                    }
                default:{
                        return GameDifficulty.unknown;
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