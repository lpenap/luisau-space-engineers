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

// Logger class, defaults to INFO
public class Logger {
	string name;
	int level;
	// 0 : error
	// 1 : warn
	// 2 : info
	// 3 : debug
	// 4 : trace
	public Logger() {
		this.name = "";
		this.level = 2;
	}
	public Logger(string name) {
		this.name = name;
		this.level = 2;
	}
	public void setLevel (int newLevel) {
		this.level = newLevel;
	}
	private void writeMsg(string msg, int msgLevel) {
		if (msgLevel >= this.level) {
			Echo (this.name + msg);
		}
	}
	public void error(string msg) {
		this.writeMsg(msg, 0);
	}
	public void warn(string msg) {
		this.writeMsg(msg, 1);
	}
	public void info(string msg) {
		this.writeMsg(msg, 2);
	}
	public void debug(string msg) {
		this.writeMsg(msg, 3);
	}
	public void trace(string msg) {
		this.writeMsg(msg, 4);
	}
}

public class Drill() {
	private int currentCycle;
	private int expectedCycles;
	public Drill() {
		// default to run 1 cycle
		this.currentCycle = 1;
		this.expectedCycles = 1;
	}
	public Drill(int currentCycle, int expectedCycles) {
		this.currentCycle = currentCycle;
		this.expectedCycles = expectedCycles;
	}
	public bool shouldRun() {
		bool shouldRun = false;
		if (this.expectedCycles > 0 && (this.currentCycle <= this.expectedCycles)) {
			shouldRun = true;
		}
		return shouldRun;
	}
	public void run() {
		if (this.shouldRun()) {
			// Running
			this.currentCycle++;
		}
	}
	public int getCurrentCycle() {
		return this.currentCycle;
	}
	public int getExpectedCycles() {
		return this.expectedCycles;
	}
	public void resetTo(int expectedCycles) {
		this.currentCycle = 1;
		this.expectedCycles = expectedCycles;
	}
}

Drill myDrill;
Logger log;
public Program() {
	log = new Logger();
	log.setLevel(3); // 3 : debug
	if (Storage.Length > 0) {
		var myVarParts = Storage.Split(";");
		int currentCycle = int.Parse(myVarParts[0]);
		int expectedCycles = int.Parse(myVarParts[1]);
		myDrill = new Drill(currentCycle, expectedCycles);
	} else {
		myDrill = new Drill();
	}
}

public void Save() {
	Storage = myDrill.getCurrentCycle() + ";" + myDrill.getExpectedCycles();
}

public void Main(string argument, UpdateType updateSource) {
	if ((updateSource & (UpdateType.Update1 | UpdateType.Update10 | UpdateType.Update100)) > 0) {
		if (myDrill.shouldRun()) {
			log.info(string.Format("Drill is going to run cycle {0} of {1}", myDrill.getCurrentCycle(), myDrill.getExpectedCycles() ));
			log.debug("Slowing down update frequency to 100");
			Runtime.UpdateFrequency = UpdateFrequency.Update100;
		} else {
			log.info("Drill is not Running!");
			log.debug("Setting frequency update to none")
			Runtime.UpdateFrequency = UpdateFrequency.None;
		}
	} else {
		// Run by user or mod, run "argument" cycles.
		log.info(string.Format("Going to run Drill for {0} cycles", argument));
		myDrill.resetTo(int.Parse(argument));
		Runtime.UpdateFrequency = UpdateFrequency.Update100;
	}
	myDrill.run();
}

// Your code ends above this line
/////////////////////////////////////////////////////////////////
#region PreludeFooter
    }
}
#endregion
