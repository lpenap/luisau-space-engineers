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
namespace SpaceEngineers.Luisau.Template {
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
		var LvdTopExtenderPrefix = "LVD TopExtender ";
		var LvdBottomExtenderPrefix = "LVD BottomExtender ";

		// Vertical Drill Piston Group
		// group for pistons that will extender the drills vertically.
		var LvdPistonsGroup = "LVD Pistons";

		// Group for Drills
		var LvdDrillsGroup = "LVD Drills";

		// Group for the Welders that will build the conveyor extension
		var LvdWeldersGroup = "LVD Welders";

		// Name for the LCD Panel that will be used to print the Drill status
		var LvdLcdStatus = "LVD Status LCD";

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
                writeMsg(msg, 2);
            }
            public void Debug(string msg) {
                WriteMsg(msg, 3);
            }
            public void Trace(string msg) {
                WriteMsg(msg, 4);
            }
        }

        public class LvdDrill {
            public LvdDrill() {
                // default to run 1 cycle
                InitDrill(1, 1);
            }

            public LvdDrill(int currentCycle, int expectedCycles) {
                InitDrill(currentCycle, expectedCycles);
            }

			public int CurrentCycle { get; private set; }

            public int ExpectedCycles { get; private set; }

			public DateTime StartTime { get; private set; }

            private void InitDrill(int currentCycle, int expectedCycles) {
                CurrentCycle = currentCycle;
                ExpectedCycles = expectedCycles;
                StartTime = System.DateTime.UtcNow;
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
                    // Running
					SimulateRun(5);
                }
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

        Drill myDrill;
        Logger log;
        public Program() {
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
            myDrill = new LvdDrill(currentCycle, expectedCycles);
            log.Debug(string.Format("Drill initialized. current cycle: {0} of {1}", myDrill.CurrentCycle, myDrill.ExpectedCycles));
        }

        public void Save() {
            Storage = myDrill.CurrentCycle + ";" + myDrill.ExpectedCycles;
        }

        public void Main(string argument, UpdateType updateSource) {
            log.info("LVD Drill Script by Luisau");
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
