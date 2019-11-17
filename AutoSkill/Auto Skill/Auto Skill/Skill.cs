using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;
using MemoryLib;
using System.Collections.Generic;
using AionMemory;
using Utility;
namespace Auto_Skill
{
	public class Skill
	{
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetTickCount();
		const float extraDelay = 1.2F;
        KeyEvents keyEvents= new KeyEvents();
        int ID;
		string name;
		string keys;
      

        List<AbilityTreeNode> nodes = null;
        HashSet<int> seenNodes = null;
		
		
		
		public Skill(int ID,string name, string keys)
		{
            this.ID = ID;
			this.name = name;
			this.keys = keys;
      
		}

        void RecurseTree(AbilityTreeNode node)
        {
            if (!seenNodes.Add(node.Ptr))
                return;

            if (node.CheckValue3 == true) 
                return;

            nodes.Add(node);

            RecurseTree(node.Left);
            RecurseTree(node.Parent); 
            RecurseTree(node.Right);
        }


        public int GetSkillTS(int Skill)
        {

            


            int headPtr = Memory.ReadInt(Process.handle, Process.Modules.Game + 0x8e3930);
            headPtr = Memory.ReadInt(Process.handle, headPtr + 0x1e4);
            AbilityTreeNode head = new AbilityTreeNode(headPtr);
            nodes = new List<AbilityTreeNode>();
            seenNodes = new HashSet<int>();
            RecurseTree(head);
           

            foreach (var node in nodes)
            {
                if (node.AbilityID == Skill)
                {
                    return node.AbilityCooldownTS;
                   
                }
            }

            return 0;
        }

		public bool Ready
		{
			get
			{
              if ((GetSkillTS(this.ID) - GetTickCount()) < 0) 
                   return true;
              else return false;


			}
		}
		
		public string Name
		{
			get
			{
				return name;
			}
		}
		
		public bool Use()
		{
          
            
         
            keyEvents.Press(keys);
            SendKeys.Send(keys);
            if (Ready) return false;
            else return true;
			
		}
	}
}
