using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepmania2BeatSaber
{
    public enum ConflictType
    {
        none,
        doubleHandConflict,
        verticalSplit,
        repeatConflict
    }
    public enum ExceptionMask
    {
        none,
        doubleOut,
        upDown
    }
    internal class RawBeat
    {
        public ArrayList RawNoteArray;
        public int Count;
        public string Mask;
        public bool HasConflict;
        public ConflictType ConflictType;
        private static readonly Dictionary<string, ConflictType> ConflictMasks = new()
        {
            { "XX00", ConflictType.doubleHandConflict },
            { "00XX", ConflictType.doubleHandConflict },
            { "0XX0", ConflictType.verticalSplit }
        };
        private static readonly Dictionary<string, ExceptionMask> ExceptionMasks = new()
        {
            { "X00X", ExceptionMask.doubleOut },
            { "0XX0", ExceptionMask.upDown },
        };
        public RawBeat()
        {
            RawNoteArray = new ArrayList();
            Count = 0;
            Mask = "";
            HasConflict = false;
            ConflictType = ConflictType.none;
        }
        public RawBeat(ArrayList rawNoteArray)
        {
            RawNoteArray = rawNoteArray;
            Count = rawNoteArray.Count;
            Mask = GetMask(rawNoteArray);
            CheckConflicts();
        }
        public void Add(RawNote note)
        {
            RawNoteArray.Add(note);
            Count++;
            Mask += GetNoteMask(note);
            if(RawNoteArray.Count > 3)
            {
                CheckConflicts();
            }
        }
        public RawNote Get(int index)
        {
            var temp = RawNoteArray[index];
            if (temp != null)
            {
                return (RawNote)temp;
            }
            throw new IndexOutOfRangeException("Provided index returned a null value: " + index.ToString());
        }
        private static string GetNoteMask(RawNote note)
        {
            if (note != null)
            {
                if (note.RawNoteType != RawNoteType.none)
                {
                    return "X";
                }
                else
                    return "0";
            }
            else
                return "0";
        }
        private static string GetMask(ArrayList RawNoteArray)
        {
            string retVal = "";
            if (RawNoteArray != null)
            {
                foreach (var item in RawNoteArray)
                {
                    retVal = GetNoteMask((RawNote)item);
                }
                return retVal;
            }
            else
                return "0000";
        }
        private void CheckConflicts()
        {
            if (ConflictMasks.ContainsKey(Mask))
            {
                ConflictType= ConflictMasks[Mask];
                HasConflict = true;
            }
            else
            {
                ConflictType = ConflictType.none;
                HasConflict = false;
            }
        }
    }
}
