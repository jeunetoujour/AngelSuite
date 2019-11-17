
using System;
using AionMemory;
using MemoryLib;
using Utility;
namespace Auto_Skill.States
{
	public abstract class State
	{
		internal StateFactory factory;
		protected Player player;
		protected Target target;
		internal KeyEvents keyEvents;
		internal IPrintable printable;
		internal SkillManager skills;
        internal FormMain FormMain;
        public string c;
        public int d;
  
        
        public State()
		{
        
		}

  
		public State Run(Player player, Target target)
		{		
			this.player = player;
            this.target = target;
           	return RunCore();
		}

		protected void Print(string text)
		{
			printable.Print(this.GetType().Name + ": " + text);
		}
		
		protected abstract State RunCore();
	}
}
