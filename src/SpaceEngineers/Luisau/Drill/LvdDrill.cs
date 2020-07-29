#region Prelude
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRageMath;
using VRage.Game;
using VRage.Collections;
using Sandbox.ModAPI.Ingame;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using Sandbox.Game.EntityComponents;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

// Change this namespace
namespace SpaceEngineers.Luisau.LvdDrill {
    public sealed class Program : MyGridProgram {

        #endregion
        /////////////////////////////////////////////////////////////////
        // Your code goes below this line


        ////////////////////////////////////////////////////////////
        // LVD Drill Script by Luisau
        // https://github.com/lpenap/luisau-space-engineers
        //
        // This script is to manage my automated drill.
        // I have yet to explain how to build it, so this Script
        // is kinda useless for the rest of the world.
        ////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////
        // Configuration:
        ////////////////////////////////////////////////////////////

        // Top/Bottom ExtenderPrefix, there should be
        // one Piston, Connector & Merge Block in each extender.
        // i.e.: There should be three blocks in the top extender:
        // "LVD TopExtender Connector",
        // "LVD TopExtender Piston", "LVD TopExtender Merge Block"
        string LvdTopExtenderPrefix = "LVD TopExtender ";
        string LvdBottomExtenderPrefix = "LVD BottomExtender ";

        // Vertical Drill Piston Group
        // group for pistons that will extender the drills vertically.
        string LvdPistonsGroup = "LVD Pistons";

        // Group for Drills
        string LvdDrillsGroup = "LVD Drills";

        // Group for the Welders that will build the conveyor extension
        string LvdWeldersGroup = "LVD Welders";

        // Name for the LCD Panel that will be used to print the Drill status
        string LvdLcdStatus = "LVD Status LCD";

        ////////////////////////////////////////////////////////////
        // End of Configuration, do not modify below this line
        ////////////////////////////////////////////////////////////

        // Logger class, defaults to INFO
        public class Logger {
            public Logger(MyGridProgram program) {
                MyProgram = program;
                Level = 2;
            }

            // 0 : error
            // 1 : warn
            // 2 : info
            // 3 : debug
            // 4 : trace
            public int Level { get; set; }

            private MyGridProgram MyProgram { get; set; }

            private void WriteMsg(string msg, int msgLevel) {
                if (msgLevel <= Level) {
                    MyProgram.Echo(msg);
                }
            }
            public void Error(string msg) {
                WriteMsg(msg, 0);
            }
            public void Warn(string msg) {
                WriteMsg(msg, 1);
            }
            public void Info(string msg) {
                WriteMsg(msg, 2);
            }
            public void Debug(string msg) {
                WriteMsg(msg, 3);
            }
            public void Trace(string msg) {
                WriteMsg(msg, 4);
            }
        }

        public class LvdDrillParts {
            public IMyPistonBase TopPistonExtender { get; set; }

            public IMyPistonBase BottomPistonExtender { get; set; }

            public IMyShipConnector TopConnectorExtender { get; set; }

            public IMyShipConnector BottomConnectorExtender { get; set; }

            public IMyShipMergeBlock TopMergeBlockExtender { get; set; }

            public IMyShipMergeBlock BottomMergeBlockExtender { get; set; }

            public IMyBlockGroup VerticalPistons { get; set; }

            public IMyBlockGroup Welders { get; set; }

            public IMyBlockGroup Drills { get; set; }

            public IMyTextPanel StatusPanel { get; set; }

            public MyGridProgram MyProgram { get; set; }
            public LvdDrillParts(MyGridProgram myProgram, string LvdTopExtenderPrefix,
                string LvdBottomExtenderPrefix, string LvdPistonsGroup,
                string LvdDrillsGroup, string LvdWeldersGroup, string LvdLcdStatus) {
                MyProgram = myProgram;

                TopConnectorExtender = (IMyShipConnector)MyProgram.GridTerminalSystem.GetBlockWithName(LvdTopExtenderPrefix + "Connector");
                TopMergeBlockExtender = (IMyShipMergeBlock)MyProgram.GridTerminalSystem.GetBlockWithName(LvdTopExtenderPrefix + "Merge Block");
                TopPistonExtender = (IMyPistonBase)MyProgram.GridTerminalSystem.GetBlockWithName(LvdTopExtenderPrefix + "Piston");

                BottomConnectorExtender = (IMyShipConnector)MyProgram.GridTerminalSystem.GetBlockWithName(LvdBottomExtenderPrefix + "Connector");
                BottomMergeBlockExtender = (IMyShipMergeBlock)MyProgram.GridTerminalSystem.GetBlockWithName(LvdBottomExtenderPrefix + "Merge Block");
                BottomPistonExtender = (IMyPistonBase)MyProgram.GridTerminalSystem.GetBlockWithName(LvdBottomExtenderPrefix + "Piston");

                VerticalPistons = MyProgram.GridTerminalSystem.GetBlockGroupWithName(LvdPistonsGroup);
                Welders = MyProgram.GridTerminalSystem.GetBlockGroupWithName(LvdWeldersGroup);
                Drills = MyProgram.GridTerminalSystem.GetBlockGroupWithName(LvdDrillsGroup);

                StatusPanel = (IMyTextPanel)MyProgram.GridTerminalSystem.GetBlockWithName(LvdLcdStatus);
            }
        }

        public class LvdDrill {

            public LvdDrill(LvdDrillParts parts) {
                // default to run 1 cycle
                InitDrill(parts, 1, 1);
            }

            public LvdDrill(LvdDrillParts parts, int currentCycle, int expectedCycles) {
                InitDrill(parts, currentCycle, expectedCycles);
            }

            public int CurrentCycle { get; private set; }

            public int ExpectedCycles { get; private set; }

            public MyGridProgram MyProgram { get; set; }

            public DateTime StartTime { get; private set; }

            public LvdDrillParts Parts { get; private set; }

            private void InitDrill(LvdDrillParts parts, int currentCycle, int expectedCycles) {
                CurrentCycle = currentCycle;
                ExpectedCycles = expectedCycles;
                StartTime = System.DateTime.UtcNow;
                MyProgram = parts.MyProgram;
                Parts = parts;
            }
            public bool ShouldRun() {
                bool shouldRun = false;
                if (ExpectedCycles > 0 && (CurrentCycle <= ExpectedCycles)) {
                    shouldRun = true;
                }
                return shouldRun;
            }
            public void Run() {
                if (ShouldRun()) {
                    CheckState();
                    // Running
                    SimulateRun(5);
                }
            }

            private void CheckState() {
                MyProgram.Echo("Top Connector: " + Parts.TopConnectorExtender.Status.ToString());
            }

            private void AdvanceCycle() {
                CurrentCycle++;
            }

            // Simulation of Drill steps each 5 seconds to test lifecycle
            private void SimulateRun(int waitSeconds) {
                DateTime currentTime = System.DateTime.UtcNow;
                TimeSpan diff = currentTime - StartTime;
                if (diff.TotalSeconds > waitSeconds) {
                    StartTime = currentTime;
                    AdvanceCycle();
                }
            }

            public void ResetTo(int expectedCycles) {
                CurrentCycle = 1;
                ExpectedCycles = expectedCycles;
            }
        }

        LvdDrill myDrill;
        Logger log;
        public Program() {
            LvdDrillParts parts = new LvdDrillParts(this, LvdTopExtenderPrefix,
                 LvdBottomExtenderPrefix, LvdPistonsGroup,
                 LvdDrillsGroup, LvdWeldersGroup, LvdLcdStatus);
            log = new Logger(this);
            log.Level = 3; // 3 : debug
            int currentCycle = 1;
            int expectedCycles = 1;
            if (Storage.Length > 0) {
                log.Debug("Loading storage: " + Storage);
                var storageParts = Storage.Split(';');
                currentCycle = int.Parse(storageParts[0]);
                expectedCycles = int.Parse(storageParts[1]);
            }
            myDrill = new LvdDrill(parts, currentCycle, expectedCycles);
            log.Debug(string.Format("Drill initialized. current cycle: {0} of {1}", myDrill.CurrentCycle, myDrill.ExpectedCycles));
        }

        public void Save() {
            Storage = myDrill.CurrentCycle + ";" + myDrill.ExpectedCycles;
        }

        public void Main(string argument, UpdateType updateSource) {
            log.Info("LVD Drill Script by Luisau");
            if ((updateSource & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) > 0) {
                if (myDrill.ShouldRun()) {
                    log.Info(string.Format("Drill is going to run cycle {0} of {1}", myDrill.CurrentCycle, myDrill.ExpectedCycles));
                    log.Debug("Slowing down update frequency to 100");
                    Runtime.UpdateFrequency = UpdateFrequency.Update100;
                } else {
                    log.Info("Drill Stopped");
                    log.Info("Run with number of cycles as Argument [defaults to 1]");
                    log.Debug("Setting frequency update to none");
                    Runtime.UpdateFrequency = UpdateFrequency.None;
                }
            } else {
                // Run by user or mod, run "argument" cycles.
                int cycles = 1;
                if (argument != "") {
                    cycles = int.Parse(argument);
                }
                log.Info(string.Format("Going to run Drill for {0} cycles", cycles));
                myDrill.ResetTo(cycles);
                Runtime.UpdateFrequency = UpdateFrequency.Update100;
            }
            myDrill.Run();
        }

        // Your code ends above this line
        /////////////////////////////////////////////////////////////////
        #region PreludeFooter
    }
}
#endregion
