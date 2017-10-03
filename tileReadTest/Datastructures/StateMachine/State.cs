using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace tileRead.Datastructures.StateMachine
{
    public interface State : IEquatable<State>
    {
        //Used so we can identify states
        int GetHashCode();
        //Useful for debugging
        string ToString();
    }

    public class Transition<T> : IEquatable<Transition<T>> where T :  new()
    {
        #region Fields
        public T transitionValue;
        private SortedList<int, State> nextStates;
        #endregion

        #region Comparators
        /// <summary>
        /// checks if the transition value is the same
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Transition<T> other)
        {
            if (other == null)
                return false;
            //check if they are literally the same object
            if (ReferenceEquals(this, other))
                return true;
            if (other.transitionValue.Equals(transitionValue))
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return transitionValue.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Transition<T>)
                return Equals(obj as Transition<T>);
            return false;
        }
        #endregion
        
        #region Constructors
        public Transition() : this(new T()){}
        public Transition(T value)
        {
            transitionValue = value;
            nextStates = new SortedList<int, State>();
        }
        public Transition(T value, State next)
        {
            transitionValue = value;
            nextStates = new SortedList<int, State>();
            nextStates.Add(next.GetHashCode(),next);
        }
        #endregion

        #region Methods
        public void addTransition(State next)
        {
            if (!nextStates.ContainsKey(next.GetHashCode()))
                nextStates.Add(next.GetHashCode(), next);
        }

        public List<State> getStatesAsList()
        {
            return nextStates.Values.ToList();
        }
        public State[] getStatesAsArray()
        {
            return nextStates.Values.ToArray();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        #endregion
    }

    public class TransitionContainer<T> : IEquatable<TransitionContainer<T>> where T : new()
    {
        #region Fields
        //The state we are "coming" from
        public State currentState;
        //The transition values from this state
        public SortedList<T, Transition<T>> transitions;
        #endregion

        #region Comparators
        /// <summary>
        /// If the state this transition starts at is that same as the state the passed in transition starts from
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(TransitionContainer<T> other)
        {
            if (other == null)
                return false;
            //check if they are literally the same object
            if (ReferenceEquals(this, other))
                return true;
            if (other.currentState.Equals(currentState))
                return true;
            return false;
        }

        public override bool Equals(Object other)
        {
            if (other is TransitionContainer<T>)
                return Equals(other as TransitionContainer<T>);
            return false;
        }

        public override int GetHashCode()
        {
            return currentState.GetHashCode();
        }
        #endregion

        #region Constructors
        public TransitionContainer(State cur)
        {
            currentState = cur;
            transitions = new SortedList<T, Transition<T>>();
        }

        /// <summary>
        /// public constructor generates an empty state list for the next possible states
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="tVal"></param>
        public TransitionContainer(State cur, T tVal)
        {
            currentState = cur;
            transitions = new SortedList<T, Transition<T>>();
            transitions.Add(tVal, new Transition<T>(tVal));
        }

        public TransitionContainer(State cur, T tVal, State fin)
        {
            currentState = cur;
            transitions = new SortedList<T, Transition<T>>();
            transitions.Add(tVal, new Transition<T>(tVal, fin));
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new transition to this current state transition.
        /// This is essentially what makes this an NFA right here.
        /// </summary>
        /// <param name="newTrans"></param>
        public void addTransition(T transitionVal, State nextState)
        {
            if(transitions.ContainsKey(transitionVal))
            {
                transitions[transitionVal].addTransition(nextState);
            }
            else
            {
                transitions.Add(transitionVal, new Transition<T>(transitionVal, nextState));
            }
        }

        /// <summary>
        /// Call to get the list of states that this state transition is capable of producing
        /// </summary>
        /// <returns></returns>
        public State[] getNextStates()
        {
            if(transitions == null)
            {
                return new State[0];
            }
            HashSet<State> retVal = new HashSet<State>();
            foreach(T key in transitions.Keys)
            {
                foreach(State s in transitions[key].getStatesAsList())
                {
                    retVal.Add(s);
                }
            }
            return retVal.ToArray();
        }


        /// <summary>
        /// Returns all possible T values that can be passed in to cause this state to transition.
        /// </summary>
        /// <returns></returns>
        public T[] getPossibleTransitionValue()
        {
            if(transitions == null)
            {
                return new T[0];
            }
            T[] retVal = new T[transitions.Count];
            int count = 0;
            foreach (T key in transitions.Keys)
            {
                retVal[count] = transitions[key].transitionValue;
                count++;
            }
            return retVal;
        }

        /// <summary>
        /// A very functional programming way of string joining
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        #endregion
    }
}