
using System;
using MemoryLib;
using AionMemory;
namespace Auto_Skill.States
{
	/// <summary>
	/// Description of Searching.
	/// </summary>
	public class Searching: State
	{
		
		
		     



    
        public bool ValidTarget()
        {
            
            if (player.TargetID == 0) return false;
            else if((target.Type  != eType.AttackableNPC) & (target.Type != eType.Gatherable)) return false;
            else if((target.Level <= 1) & (target.Type == eType.AttackableNPC)) return false;
            return true;
            
        }

        
        
		protected override State RunCore()
		{

            if(ValidTarget())
			{
               return factory.NewState<Fighting>();
			}
			
			return this;
		}
	}
}
