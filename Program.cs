using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    static class Program{
        private static readonly string pDir = @"C:\src\BeatSaber\BREAK DOWN!\original";
        private static readonly string pFilename = "BREAK DOWN!.sm";
        private static readonly string pSongName = pFilename.Split(".")[0];
        private static readonly string pChroMapperVersion = "2.2.0";
        public static void Main(){
            OrderedDictionary hash = ReadFile();
            if (hash != null && hash.Keys.Count > 0){
                var temp = hash["bpm"];
                if (temp != null){
                    Output("BPM Found: " + ((double)temp).ToString(), ConsoleColor.Green);
                }
                temp = hash["songs"];
                 if(temp!= null){
                    var songs = ParseSong((OrderedDictionary)temp);
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
                    foreach (Note note in (ArrayList)temp)
                        notes.Add(note.ToJOject());
                    //----------
                    JObject chroMapperJSON = FormatJSON(pChroMapperVersion, notes);
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
                return (string) retVal;
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
                    while (reader != null && !reader.EndOfStream){
                        line = GetNextLine(reader);
                        if (line != null && line.StartsWith("//") && line.IndexOf("dance-single") > -1){
                            reader.ReadLine();
                            reader.ReadLine();
                            reader.ReadLine();
                            // Get Difficulty
                            string difficulty = FindDifficulty(GetNextLine(reader));
                            int noteCount = 0;
                            Output("//Found Difficulty - " + difficulty + " ---------------------------------", ConsoleColor.Yellow);
                            reader.ReadLine();
                            reader.ReadLine();
                            ArrayList notesByDifficulty = new();
                            while (!reader.EndOfStream){
                                var newDifficulty = false;
                                // Get Noteset
                                ArrayList noteSet = new();
                                while (!reader.EndOfStream){
                                    line = GetNextLine(reader);
                                    if (line != null){
                                        if (!line.StartsWith(",") && !line.StartsWith(";")){
                                            noteSet.Add(line);
                                            noteCount += 1;
                                            Output("Note#" + noteCount.ToString("D3") + ":  " + line, ConsoleColor.Yellow);
                                        }
                                        else{
                                            if (line.StartsWith(";"))
                                                newDifficulty = true;
                                            break;
                                        }
                                    }
                                }
                                notesByDifficulty.Add(noteSet);
                                if (newDifficulty)
                                    break;
                            }
                            playCollection.Add(difficulty, notesByDifficulty);
                        }
                        else if (line != null && line.StartsWith("#DISPLAYBPM:")){
                            line = line.Replace("#DISPLAYBPM:", "");
                            line = line.Replace(";", "");
                            retHash.Add("bpm", double.Parse(line.Trim()));
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
        public static OrderedDictionary ParseSong(OrderedDictionary allData){
            OrderedDictionary retVal = new();
            foreach (string key in allData.Keys){
                Output("Creating song: " + key, ConsoleColor.Cyan);
                ArrayList notesByDifficulty = new();
                if (allData != null && allData[key] != null)
                    try{
                        var temp = allData[key];
                        if(temp != null)
                            notesByDifficulty = (ArrayList)temp;
                    }
                    catch{
                        notesByDifficulty = new();
                    }
                ArrayList retArray = new();
                double baseBeats = 0;
                if(notesByDifficulty != null){
                    foreach (ArrayList noteSet in notesByDifficulty){
                        // Default is 4 notes per measure - we may have more, so calculate the interval
                        double interval = 4.0 / (double)noteSet.Count;
                        if (!interval.Equals(1))
                            Output("Found a different interval: " + interval.ToString(), ConsoleColor.Magenta);
                        foreach (string noteString in noteSet){
                            if (noteString.Length != 4)
                                throw new NotImplementedException("New Note Count found - expected only 4");
                            // split the noteset into notes
                            int count = 0;
                            char[] noteCharArray = noteString.ToCharArray();
                            while (count < noteString.Length){
                                char c = noteCharArray[count];
                                if (!(c.Equals('0'))){
                                    Note note = new();
                                    if (count > 1)
                                        note.Type = Type.blue;
                                    switch (count){
                                        case 0:{
                                                note.CutDirection = CutDirection.left;
                                                note.LineIndex = LineIndex.left;
                                                break;
                                            }
                                        case 1:{
                                                note.CutDirection = CutDirection.down;
                                                note.LineIndex = LineIndex.centerLeft;
                                                break;
                                            }
                                        case 2:{
                                                note.CutDirection = CutDirection.up;
                                                note.LineIndex = LineIndex.centerRight;
                                                break;
                                            }
                                        case 3:{
                                                note.CutDirection = CutDirection.right;
                                                note.LineIndex = LineIndex.right;
                                                break;
                                            }
                                        default:{
                                                throw new NotImplementedException("Too many notes. Something is critically wrong.");
                                            }
                                    }
                                    note.Time = baseBeats;
                                    retArray.Add(note);
                                    if (!(c.Equals('1'))){
                                        Output("Found an unexpected note value:" + noteCharArray[count].ToString() + " - Something is wrong.", ConsoleColor.Magenta);
                                    }
                                }
                                count += 1;
                            }
                            baseBeats += interval;
                        }
                    }
                    retVal.Add(key, retArray);
                }   
            }
            return retVal;
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
        public static JObject FormatJSON(string version, JArray notes){
            return new JObject(new JProperty("_version", version),
                new JProperty("_notes", notes),
                new JProperty("_obstacles", new JArray()),
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