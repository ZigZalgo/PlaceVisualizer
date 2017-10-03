using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using tileRead.Datastructures.StateMachine;
using tileRead.Datastructures;

namespace tileRead.Tools
{
    public class PositionState : State
    {
        #region Fields
        Entry input;
        #endregion

        #region Comparators
        //Check if two position states are equal
        public override bool Equals(Object other)
        {
            if (other is PositionState)
                return Equals(other as PositionState);
            return false;
        }

        public bool Equals(PositionState other)
        {
            if (other == null)
                return false;
            //check if they are literally the same object
            if (ReferenceEquals(this, other))
                return true;
            return (other.input.x.Equals(input.x) && other.input.y.Equals(input.y));
        }

        public bool Equals(State other)
        {
            if (other is PositionState)
                return Equals(other as PositionState);
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 71 * hash + input.x;
            hash = 71 * hash + input.y;
            return hash;
        }
        #endregion

        #region Constructors
        public PositionState(Entry e)
        {
            input = e;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion

        #region Static Methods
        //First define the delegates needed
        public static SortedList<int, TransitionContainer<int>> genPositionTransitionTable(Entry[] entries)
        {
            SortedList<int, TransitionContainer<int>> fastVal = new SortedList<int, TransitionContainer<int>>();
            DebugLog.LogConsole("Beginning to add states to the transition table");
            for (int i = 0; i < entries.Count(); i++)
            {
                if (i > 0)
                {
                    if (i % 500000 == 0)
                    {
                        DebugLog.LogConsole(i + " states parsed.");
                        DebugLog.LogConsole(fastVal.Count + " unique states");
                    }
                }

                if (i == entries.Count() - 1)
                {
                    return fastVal;
                }

                PositionState curEntry = genPositionState(entries[i]);
                if (fastVal.ContainsKey(curEntry.GetHashCode()))
                {
                    PositionState nextEntry = genPositionState(entries[i + 1]);
                    int transVal = entries[i + 1].colourID;
                    fastVal[curEntry.GetHashCode()].addTransition(transVal, nextEntry);
                }
                else
                {
                    TransitionContainer<int> toAdd = new TransitionContainer<int>(curEntry);
                    fastVal.Add(toAdd.currentState.GetHashCode(), toAdd);
                }
            }
            DebugLog.LogConsole("Final unqiue state count : "+fastVal.Count);
            return fastVal;
        }

        public static PositionState genPositionState(Entry entry)
        {
            return new PositionState(entry);
        }
        #endregion
    }

    public class ColorState : State
    {
        #region Fields
        Entry input;
        #endregion

        #region Comparators
        //Check if two position states are equal
        public override bool Equals(Object other)
        {
            if (other is ColorState)
                return Equals(other as ColorState);
            return false;
        }

        public bool Equals(ColorState other)
        {
            if (other == null)
                return false;
            //check if they are literally the same object
            if (ReferenceEquals(this, other))
                return true;
            return (other.input.colourID.Equals(input.colourID));
        }


        public bool Equals(State other)
        {
            if (other is ColorState)
                return Equals(other as ColorState);
            return false;
        }

        public override int GetHashCode()
        {
            return input.colourID;
        }
        #endregion

        #region Constructors
        public ColorState(Entry e)
        {
            input = e;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion

        #region Static Methods
        //First define the delegates needed
        public static SortedList<int, TransitionContainer<int>> genPositionTransitionTable(Entry[] entries)
        {
            SortedList<int, TransitionContainer<int>> fastVal = new SortedList<int, TransitionContainer<int>>();
            DebugLog.LogConsole("Beginning to add states to the transition table");
            for (int i = 0; i < entries.Count(); i++)
            {
                if (i > 0)
                {
                    if (i % 1000000 == 0)
                    {
                        DebugLog.LogConsole(i + " states added.");
                    }
                }

                if (i == entries.Count() - 1)
                {
                    return fastVal;
                }

                ColorState curEntry = genColorState(entries[i]);
                if (fastVal.ContainsKey(curEntry.GetHashCode()))
                {
                    ColorState nextEntry = genColorState(entries[i + 1]);
                    int transVal = 7;
                    transVal = 71 * transVal + entries[i + 1].x;
                    transVal = 71 * transVal + entries[i + 1].y;
                    fastVal[curEntry.GetHashCode()].addTransition(transVal, nextEntry);
                }
                else
                {
                    TransitionContainer<int> toAdd = new TransitionContainer<int>(curEntry);
                    fastVal.Add(toAdd.currentState.GetHashCode(), toAdd);
                }
            }
            return fastVal;
        }

        public static ColorState genColorState(Entry entry)
        {
            return new ColorState(entry);
        }
        #endregion
    }
}

