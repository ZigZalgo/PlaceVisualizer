
using System;
using System.IO;
using tileRead.Datastructures;
using tileRead.Datastructures.StateMachine;
using tileRead.Tools;
using static tileRead.Datastructures.StateMachine.StateMachine<int>;

namespace tileRead.Tests
{
    public class MachineSetupTest
    {
        static GEN_TRAN_TABLE tanTable = PositionState.genPositionTransitionTable;
        static GEN_STATE_FROM_ENTRY stateGen = PositionState.genPositionState;
        /// <summary>
        /// Args0 = NAME NOT PATH to read
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            DebugLog.resetLog();
            DebugLog.setDebug(true);

            if (!FileStructure.CheckIfFileParsed(args[0]))
            {
                DebugLog.LogConsole("File " + args[0] + " does not exist in cache.");
                throw new IOException("File " + args[0] + " does not exist in cache.");
            }
            DebugLog.LogConsole("Reading entries from binary.");
            Entry[] entries = Entry.readEntriesFromFile(FileStructure.parseDirectory + args[0] + ".bin");

            DebugLog.LogConsole("Generating state machine.");
            StateMachine<int> machine = new StateMachine<int>(entries, ref tanTable, ref stateGen);
            machine.StartMachine();
        }        
    }
}
