using System;
using System.Collections.Generic;
using System.Linq;
using tileRead.Datastructures;
using tileRead.Tools;

namespace tileRead.Datastructures.StateMachine
{
    public class NFAException : Exception
    {
        public NFAException()
            : base() { }

        public NFAException(string message)
            : base(message) { }

        public NFAException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public NFAException(string message, Exception innerException)
            : base(message, innerException) { }

        public NFAException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }


    /// <summary>
    /// An NFA machine.
    /// (Q, Sigma, Delta, q0, F)
    /// (set of all states, input symbols, transition functions, start state, final state(singular because our data only HAS one final state)
    /// </summary>
    /// <typeparam name="T">Sigma</typeparam>
    public class StateMachine<T> where T : new()
    {
        #region Delegate Definitions
        /// <summary>
        /// The Delta of the NFA machine
        /// Delegate for creating the transition table.
        /// Given a set of entries, defines how to generate states for the entries as well as the state transitions and the relationship between the two
        /// </summary>
        /// <param name="entries"></param>
        /// <returns></returns>
        public delegate SortedList<int, TransitionContainer<T>> GEN_TRAN_TABLE(Entry[] entries);
        /// <summary>
        /// Delegate for creating States from entries
        /// used for generating (q ∈ Q)
        /// </summary>
        /// <param name="toConvert"></param>
        /// <returns></returns>
        public delegate State GEN_STATE_FROM_ENTRY(Entry toConvert);
        /// <summary>
        /// Delegate for deciding how to choose between multiple posible states correspondiong to a state transitions state change
        /// </summary>
        /// <param name="possibleOuts">the possible states to transition to</param>
        /// <returns></returns>
        public delegate State TIE_BREAKER(State[] possibleOuts);
        #endregion
        //The only public var as its the only one we will be referencing for our visualizations
        public State currentState;
        private State previousState;
        //q0
        private State startState;
        //F
        private State finalState;
        public SortedList<int, TransitionContainer<T>> transitionTable;
        /// <summary>
        /// Creates the machine.
        /// Works in a very functional programming way in that it has two higher order functions being used as arguments
        /// </summary>
        /// <param name="entries">the array of all entries the machine is to be generated from</param>
        /// <param name="tableGenerator">a delegate methid</param>
        /// <param name="stateGenerator"></param>
        public StateMachine(Entry[] entries, ref GEN_TRAN_TABLE tableGenerator, ref GEN_STATE_FROM_ENTRY stateGenerator)
        {
            DebugLog.LogConsole("Generating transition table.");
            transitionTable = tableGenerator(entries);
            DebugLog.LogConsole("Transition table generated.");
            startState = stateGenerator(entries[0]);
            finalState = stateGenerator(entries[entries.Length - 1]);
        }

        /// <summary>
        /// Places our current state as the start state
        /// </summary>
        public void StartMachine()
        {
            DebugLog.LogConsole("Machine Started.");
            currentState = startState;
        }

        /// <summary>
        /// Get the transitionstate structure which contains all the transition structures possible for this state
        /// </summary>
        /// <returns></returns>
        private TransitionContainer<T> getTransitionForCurrentState()
        {
            if (currentState == null)
            {
                DebugLog.LogConsole("There does not exist a current state currently.");
                throw new NFAException("There does not exist a current state currently.");
            }
            return transitionTable[currentState.GetHashCode()];
        }

        /// <summary>
        /// returns all possible transition values that cause this state to transition to another.
        /// If a value does not cause this state to change, it will not be returned
        /// </summary>
        /// <returns></returns>
        private T[] getPossibleTransitionValuesForCurrentState()
        {
            try
            {
                TransitionContainer<T> trans = getTransitionForCurrentState();
                return trans.getPossibleTransitionValue();
            }
            catch (Exception)
            {
                DebugLog.LogConsole("It is not possible to transition out of our current state.");
                throw new NFAException("It is not possible to transition out of our current state.");
            }
        }


        private State[] getPossibleNextStatesForTransition(T transitionVal)
        {
            TransitionContainer<T> container = transitionTable[currentState.GetHashCode()];
            State[] states = container.transitions[transitionVal].getStatesAsArray();
            if (states.Length < 1)
            {
                DebugLog.LogConsole("There did not exist a transition of " + transitionVal.ToString() + " for the current state.");
                throw new NFAException("There did not exist a transition of " + transitionVal.ToString() + " for the current state.");
            }
            else
                return states;
        }

        /// <summary>
        /// Takes the machine from its current state to the next state based on this input.
        /// Because this is an NFA, there is a delegate here for how the user would like 
        /// to choose between multiple states for the next state should multiple states
        /// be an option for the given input.
        /// Returns true if a final state has been reached
        /// </summary>
        /// <param name="input"></param>
        public bool Transition(T input, ref TIE_BREAKER tieBreakers)
        {
            //set our previous state to be this one
            previousState = currentState;
            //and use our delegate to decide how we want to choose our next state
            currentState = tieBreakers(getPossibleNextStatesForTransition(input));
            return checkIfFinalState();
        }

        /// <summary>
        /// Used for determining if we have reached the final state
        /// </summary>
        /// <returns></returns>
        private bool checkIfFinalState()
        {
            return finalState.Equals(currentState);
        }
    }
}
