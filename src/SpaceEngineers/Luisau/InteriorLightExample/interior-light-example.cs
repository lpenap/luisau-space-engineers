#region Program Header
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

// Example program to toggle an interior light called "Interior Light"
// every 200 ticks
namespace SpaceEngineers.Luisau.InteriorLightExample
{
    public sealed class Program : MyGridProgram
    {

        #endregion
        /////////////////////////////////////////////////////////////////
        // Your code goes below this line

        string lightName = "Interior Light";

        int _count;
        public Program()
        {
            if (Storage.Length > 0)
            {
                _count = int.Parse(Storage);
            }
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            Echo("Program constructed");
        }

        public void Save()
        {
            Storage = _count + "";
            Echo("Program saved");
        }

        public void Main(string args)
        {
            IMyInteriorLight light = (IMyInteriorLight)GridTerminalSystem.GetBlockWithName(lightName);
            if (_count % 2 == 0)
            {
                Echo("Toggle light!");
                light.ApplyAction("OnOff");
                _count = 0;
            }
            Echo("executing, " + (_count++));
        }

        // Your code ends above this line
        /////////////////////////////////////////////////////////////////
        #region Program Footer
    }
}
#endregion