using Newtonsoft.Json.Linq;

namespace Stepmania2BeatSaber
{
    public enum IsLeftOrRightSide
    {
        none,
        left,
        right
    }

    public enum RawDirection
    {
        none,
        up,
        down,
        left,
        right
    }
    public enum RawNoteType
    {
        none,
        normal,
        three,
        four,
        unknown
    }

    public class RawNote
    {
        public RawNote()
        {
            IsLeftOrRightSide = IsLeftOrRightSide.none;
            RawDirection = RawDirection.none;
            RawNoteType = RawNoteType.none;
        }
        public RawNote(string noteString, Int32 index)
        {
            char noteType = '0';
            if(noteString != null)
            {
                noteType= noteString[index];
            }
            DetermineRawNoteType(noteType);
            if (RawNoteType != RawNoteType.none)
            {
                switch (index)
                {
                    case 0:
                        {
                            IsLeftOrRightSide = IsLeftOrRightSide.left;
                            RawDirection = RawDirection.left;
                            break;
                        }
                    case 1:
                        {
                            IsLeftOrRightSide = IsLeftOrRightSide.left;
                            RawDirection = RawDirection.down;
                            break;
                        }
                    case 2:
                        {
                            IsLeftOrRightSide = IsLeftOrRightSide.right;
                            RawDirection = RawDirection.up;
                            break;
                        }
                    case 3:
                        {
                            IsLeftOrRightSide = IsLeftOrRightSide.left;
                            RawDirection = RawDirection.right;
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("Error - unexpected case.");
                        }
                }
            }
        }
        private void DetermineRawNoteType(char noteType)
        {
            if(noteType.Equals('0'))
            {
                RawNoteType = RawNoteType.none;
            }
            else if (noteType.Equals('1'))
            {
                RawNoteType = RawNoteType.normal;
            }
            else if (noteType.Equals('3'))
            {
                RawNoteType = RawNoteType.three;
            }
            else if (noteType.Equals('4'))
            {
                RawNoteType = RawNoteType.four;
            }
            else
            {
                RawNoteType = RawNoteType.unknown;
                Stepmania2BeatSaber.Output("Found a new note type: " + noteType.ToString(), ConsoleColor.Red);
            }
        }
        public IsLeftOrRightSide IsLeftOrRightSide { get; set; }
        public RawDirection RawDirection { get; set; }
        public RawNoteType RawNoteType { get; set; }
}
}