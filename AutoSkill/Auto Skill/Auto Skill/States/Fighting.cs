using System.Windows.Forms;
using System;

namespace Auto_Skill.States
{
	public class Fighting: State
	{
		Random random = new Random();
		
		bool firstAttack = true;
		
		int pullCycle = 0;
		int attackCycle = 0;
        int chainCycle = 0;
        int healCycle = 0;
        bool successuse = false;
       
        bool chainReady = true;
        int curChain = -1;
        int timesFailed = 0;
		



        public bool Wounded(double percentage)
        {
            if (player.Health <= player.MaxHealth * percentage) return true;
            return false;

        }


		protected override State RunCore()
		{
			if (player.TargetID == 0)
			{
				return factory.NewState<Searching>();
			}
			
         
			if (target.Health > 0)
			{
				
             
                if (Wounded(0.5))
                {
                    if (skills.Heals.Count > 0)
                    {
                        if (skills.Heals.skills[healCycle].Ready)
                        {
                            successuse = skills.Heals.skills[healCycle].Use();
                            if ((successuse))
                            {
                                Print("Healing With " + skills.Heals.skills[healCycle].Name);

                                healCycle += 1;
                                if (healCycle >= skills.Heals.Count) healCycle = 0;

                            }
                        }
                        else
                        {
                            healCycle += 1;
                            if (healCycle >= skills.Heals.Count) healCycle = 0;

                        }

                    }
                }

                


                if (firstAttack)
                {
                   
           
                    if (skills.Pulls.Count > 0)
                    {

                        successuse = skills.Pulls.skills[pullCycle].Use();


                        if (successuse)
                        {
                            Print("Pulling With " + skills.Pulls.skills[pullCycle].Name);
                            pullCycle += 1;
                            if (pullCycle >= skills.Pulls.Count)
                                firstAttack = false;

                        }

                    }
                    else
                    {
                        firstAttack = false;
                    }

                }

                else
                {


                    if (skills.Chains[chainCycle].Count > 0)
                    {

                        if (curChain != chainCycle)//add check for if one chain only
                        {

                            if (skills.Chains[chainCycle].skills[0].Ready)
                            {

                                chainReady = true;
                                curChain = chainCycle;
                            }
                            else
                            {
                                Print("" + skills.Chains[chainCycle].skills[0].Name + " Is Not Ready! Skipping!");
                                curChain = -1;
                                chainReady = false;
                                attackCycle = 0;
                                chainCycle += 1;
                                if (chainCycle >= skills.Count) chainCycle = 0;

                            }
                        }

                        if (chainReady)
                        {


                            successuse = skills.Chains[chainCycle].skills[attackCycle].Use();

                            if ((successuse) | (timesFailed > 20))
                            {
                                if (successuse) Print("Using " + skills.Chains[chainCycle].skills[attackCycle].Name);
                                timesFailed = 0;
                                attackCycle += 1;
                                if (attackCycle >= skills.Chains[chainCycle].Count)
                                {
                                    curChain = -1;
                                    chainReady = false;
                                    attackCycle = 0;
                                    chainCycle += 1;
                                    if (chainCycle >= skills.Count) chainCycle = 0;
                                }

                            }
                            else
                            {
                                if (timesFailed > 18) Print("Failed Using " + skills.Chains[chainCycle].skills[attackCycle].Name);
                                timesFailed += 1;
                            }

                        }
                    }
                    else
                    {
                        
                          SendKeys.Send("c"); // sendkeys O.o
                      //  keyEvents.Press("c");
                           chainCycle += 1;
                           if (chainCycle >= skills.Count) chainCycle = 0;

                        
                    }
                        
                        

                    }

                

               
			}
			else
			{
				return factory.NewState<Looting>();
			}
			
			return this;
		}
	}
}
