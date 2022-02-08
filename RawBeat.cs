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
        repeatConflict,
        leftSameLayer,
        rightSameLayer
    }
    public enum RepeatException
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
        public RepeatException RepeatException;
        public double BeatTime { get; set; }
        private static readonly Dictionary<string, ConflictType> ConflictMasks = new()
        {
            { "XX00", ConflictType.doubleHandConflict },
            { "00XX", ConflictType.doubleHandConflict },
            { "0XX0", ConflictType.verticalSplit },
            { "X0X0", ConflictType.leftSameLayer },
            { "0X0X", ConflictType.rightSameLayer }
        };
        private static readonly Dictionary<string, RepeatException> RepeatMasks = new()
        {
            { "X00X", RepeatException.doubleOut },
            { "0XX0", RepeatException.upDown },
        };
        public RawBeat()
        {
            RawNoteArray = new ArrayList();
            Count = 0;
            Mask = "";
            HasConflict = false;
            ConflictType = ConflictType.none;
            BeatTime = 0;
        }
        public RawBeat(ArrayList rawNoteArray, double bTime)
        {
            RawNoteArray = rawNoteArray;
            Count = rawNoteArray.Count;
            Mask = GetMask(rawNoteArray);
            BeatTime = bTime;
            CheckConflictsAndExceptions();
        }
        public void Add(RawNote note)
        {
            RawNoteArray.Add(note);
            Count++;
            Mask += GetNoteMask(note);
            if (RawNoteArray.Count > 3)
            {
                CheckConflictsAndExceptions();
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
        private void CheckConflictsAndExceptions()
        {
            if (ConflictMasks.ContainsKey(Mask))
            {
                ConflictType = ConflictMasks[Mask];
                HasConflict = true;
            }
            else
            {
                ConflictType = ConflictType.none;
                HasConflict = false;
            }
            //---------------------------------------
            if (RepeatMasks.ContainsKey(Mask))
            {
                RepeatException = RepeatMasks[Mask];
                HasConflict = true;
            }
            else
            {
                RepeatException = RepeatException.none;
                HasConflict = false;
            }
        }
    }
}
