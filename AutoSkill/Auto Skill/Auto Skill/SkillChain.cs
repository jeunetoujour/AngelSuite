using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auto_Skill
{
    public class SkillChain
    {

        public Skill[] skills;
        public int Count;

        public SkillChain(int numskills, Skill[] skillarray)
        {
            skills = skillarray;
            Count = numskills;

        }

    }
}
