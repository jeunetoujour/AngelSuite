using System.Windows.Forms;
using System;

namespace Auto_Skill.States
{
	/// <summary>
	/// Description of Looting.
	/// </summary>
	public class Looting: State
	{
		DateTime? lootAttempt = new Nullable<DateTime>();
		int lootAttemptCount = 0;
		
		protected override State RunCore()
		{
            
			if (!target.HasTarget || lootAttemptCount > 3)
			{
				return factory.NewState<Searching>();
			}
			
			if (!lootAttempt.HasValue || DateTime.Now > lootAttempt.Value) //Should check memory offset if target is dead/gone/lootable! cant be bothered!
			{
                if (lootAttemptCount == 0) Print("Looting The Decaying Carcass Of A Once Active Yet Disgusting Being!");
				lootAttempt = DateTime.Now.AddSeconds(2);
				lootAttemptCount++;
                keyEvents.Press(skills.LootKey);				
			}
			
			return this;
		}
	}
}
