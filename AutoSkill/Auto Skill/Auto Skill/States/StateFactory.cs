
using System;
using Utility;

namespace Auto_Skill.States
{
	/// <summary>
	/// Description of StateFactory.
	/// </summary>
	public class StateFactory
	{
		KeyEvents keyEvents;
		IPrintable printable;
		SkillManager skills;
        FormMain FormMain;
		public StateFactory(FormMain FormMain,KeyEvents keyEvents, SkillManager skills, IPrintable printable)
		{
            this.FormMain = FormMain;
			this.keyEvents = keyEvents;
			this.printable = printable;
			this.skills = skills;
		}

        public State NewState<T>() where T : State, new()
		{
			T result = new T();
			result.factory = this;
			result.keyEvents = keyEvents;
			result.printable = printable;
			result.skills = skills;
            result.FormMain = this.FormMain;
			return result;
		}
	}
}
