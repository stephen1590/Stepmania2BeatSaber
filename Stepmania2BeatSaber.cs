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
        private static readonly string pChroMapperVersion = "2.2.0";
        public static void Main(){
            double bpm = 0.0;
            double offset = 0.0;
            OrderedDictionary hash = ReadFile();
            if (hash != null && hash.Keys.Count > 0){
                var temp = hash["offset"];
                if (temp != null){
                    offset = (double)temp;
                    Output("Offset Found: " + offset.ToString(), ConsoleColor.Green);
                }
                temp = hash["bpm"];
                if (temp != null)
                {
                    bpm = (double)temp;
                    Output("BPM Found: " + bpm.ToString(), ConsoleColor.Green);
                }
                temp = hash["songs"];
                if (temp != null){
                    var songs = ParseSong((OrderedDictionary)temp, offset, bpm);
                    WriteFile(songs);
                }
            }
            Output("Press any key to exit...");
            Console.ReadKey();
        }
        public static void WriteFile(OrderedDictionary objectToWrite){
            string basefilename = "Standard.dat";
            string newPath = pDir + @"\BeatSaber - " + pSongName + @"\";
            foreach (string key in objectToWrite.Keys){
                string filename = newPath + key + basefilename;
                // ---------
                JArray notes = new();
                var temp = objectToWrite[key];

                if (temp != null){
                    foreach (BSaberNote note in (ArrayList)temp)
                        notes.Add(note.ToJOject());
                    //----------
                    JObject chroMapperJSON = FormatJSON(pChroMapperVersion, notes, new JArray());
                    // ---------
                    if (!Directory.Exists(newPath))
                        Directory.CreateDirectory(newPath);
                    //----------
                    using (StreamWriter writer = new(filename, false)){
                        writer.Write(chroMapperJSON.ToString());
                    }
                    Output("Wrote File: " + filename, ConsoleColor.Cyan);
                }
            }
        }
        public static string GetNextLine(StreamReader sr){
            try{
                var retVal = sr.ReadLine();
                if (retVal == null)
                    return "";
                return (string)retVal;
            }
            catch{
                return "";
            }
        }
        public static OrderedDictionary ReadFile(){
            Output("Reading data...", ConsoleColor.Cyan);
            OrderedDictionary playCollection = new();
            OrderedDictionary retHash = new();
            string line;
            string filename = pDir + @"\" + pFilename;
            try{
                using (StreamReader reader = new(filename)){
                    // Read one line from file
                    Int32 foundItems = 0;
                    while (reader != null && !reader.EndOfStream && !foundItems.Equals(2))
                    {
                        line = GetNextLine(reader);
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
                        line = GetNextLine(reader);
                        if (line != null && line.StartsWith("//") && line.IndexOf("dance-single") > -1){
                            reader.ReadLine();
                            reader.ReadLine();
                            reader.ReadLine();
                            // Get Difficulty
                            string difficulty = FindDifficulty(GetNextLine(reader));
                            int beatcount = 0;
                            Output("//Found Difficulty - " + difficulty + " ---------------------------------", ConsoleColor.Yellow);
                            reader.ReadLine();
                            reader.ReadLine();
                            ArrayList notesByDifficulty = new();
                            while (!reader.EndOfStream){
                                var newDifficulty = false;
                                // Get Noteset
                                ArrayList measure = new();
                                while (!reader.EndOfStream){
                                    line = GetNextLine(reader);
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
                                            Output("Note#" + beatcount.ToString("D3") + ":  " + line, ConsoleColor.Yellow);
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
                Output("Error reading file. " + Environment.NewLine + ex.Message, ConsoleColor.Red);
            }
            return retHash;
        }
        public static OrderedDictionary ParseSong(OrderedDictionary allData, double offset, double bpm)
        {
            OrderedDictionary retVal = new();
            foreach (string key in allData.Keys){
                Output("Creating song: " + key, ConsoleColor.Cyan);
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
                bool repeatBeat = false;
               
                //
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
                                    Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta);
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
                            Output("Found an unexpected note value:" + r.RawNoteType.ToString(), ConsoleColor.Magenta);
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
        public static string FindDifficulty(string difficulty){
            switch (difficulty.Split(":")[0].Trim()){
                case "Beginner":{
                        return "Easy";
                    }
                case "Easy":{
                        return "Normal";
                    }
                case "Medium":{
                        return "Hard";
                    }
                case "Hard":{
                        return "Expert";
                    }
                default:{
                        return "";
                    }
            }
        }
        public static JObject FormatJSON(string version, JArray notes, JArray obstacles){
            return new JObject(new JProperty("_version", version),
                new JProperty("_notes", notes),
                new JProperty("_obstacles", obstacles),
                new JProperty("_events", new JArray()),
                new JProperty("_waypoints", new JArray()));
        }
        public static void Output(string outputString, ConsoleColor color){
            Console.ForegroundColor = color;
            Console.WriteLine(outputString);
            Console.ResetColor();
        }
        public static void Output(string outputString){
            Console.WriteLine(outputString);
        }
    }
}