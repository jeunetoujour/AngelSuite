
using System;
using Utility;

namespace Auto_Skill
{
	public class SkillManager
	{
	
        FormMain FormMain;
        private string SkillSC = "0";
        private int SkillID = 0;
       public SkillChain[] Chains; 
       public SkillChain Pulls; 
       public SkillChain Heals;
        public string SitKey;
        public string LootKey;
        public string HealKey;
        private int cnt;
      public int Count;
        
        Skill[] Combo;
		
		public SkillManager(KeyEvents keyEvents, FormMain FormMain)
		{
			
            this.FormMain = FormMain;
            LootKey = FormMain.skilltree.Nodes[0].Nodes[0].Nodes[0].Text;
            SitKey = FormMain.skilltree.Nodes[0].Nodes[1].Nodes[0].Text;
            HealKey = FormMain.skilltree.Nodes[0].Nodes[2].Nodes[0].Text;
            Chains = new SkillChain[FormMain.skilltree.Nodes.Count-3];
          
            Count = FormMain.skilltree.Nodes.Count-3;

            Combo = new Skill[FormMain.skilltree.Nodes[1].Nodes.Count]; 
            for (cnt = 0; cnt < FormMain.skilltree.Nodes[1].Nodes.Count; cnt++)
            {
                
                SkillSC = FormMain.skilltree.Nodes[1].Nodes[cnt].Nodes[0].Text;
                SkillID = Convert.ToInt32(FormMain.skilltree.Nodes[1].Nodes[cnt].Tag);
                Attack = new Skill(SkillID, FormMain.skilltree.Nodes[1].Nodes[cnt].Text, SkillSC);
                Combo[cnt] = Attack;
            }
            Pulls = new SkillChain(FormMain.skilltree.Nodes[1].Nodes.Count, Combo);



            Combo = new Skill[FormMain.skilltree.Nodes[2].Nodes.Count];
            for (cnt = 0; cnt < FormMain.skilltree.Nodes[2].Nodes.Count; cnt++)
            {

                SkillSC = FormMain.skilltree.Nodes[2].Nodes[cnt].Nodes[0].Text;
                SkillID = Convert.ToInt32(FormMain.skilltree.Nodes[2].Nodes[cnt].Tag);
                Attack = new Skill(SkillID, FormMain.skilltree.Nodes[2].Nodes[cnt].Text, SkillSC);
                Combo[cnt] = Attack;
            }
            Heals = new SkillChain(FormMain.skilltree.Nodes[2].Nodes.Count, Combo);





            for (int ctr = 3; ctr < FormMain.skilltree.Nodes.Count; ctr++)
            {
                Combo = new Skill[FormMain.skilltree.Nodes[ctr].Nodes.Count]; 
                for (cnt = 0; cnt < FormMain.skilltree.Nodes[ctr].Nodes.Count; cnt++)
                {
                    
                    SkillSC = FormMain.skilltree.Nodes[ctr].Nodes[cnt].Nodes[0].Text;
                    SkillID = Convert.ToInt32(FormMain.skilltree.Nodes[ctr].Nodes[cnt].Tag);
                    Attack = new Skill(SkillID, FormMain.skilltree.Nodes[ctr].Nodes[cnt].Text, SkillSC);
                    Combo[cnt] = Attack;
                }
                Chains[ctr-3] = new SkillChain(FormMain.skilltree.Nodes[ctr].Nodes.Count, Combo);
            }


          
            #region BS
            /*
			Attack = new Skill("Attack", 0, 0, delegate
			                   {
			                   	keyEvents.Press('1');
			                   });
			SwiftEdge = new Skill("Swift Edge", 0, 7, delegate
			                      {
			                      	keyEvents.Press('2');
			                      });
			FocusedEvasion = new Skill("Focused Evasion", 0, 30, delegate
			                           {
			                           	keyEvents.Press('3');
			                           });

           // Pull = new Skill[20];
			Pull = new Skill[] {
				Attack
			};
			
			Fight = new Skill[] {
				SwiftEdge,
				FocusedEvasion
			};
             */
            #endregion

        }
		
		public Skill Attack;
		
        
	}
}
