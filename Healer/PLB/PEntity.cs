using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AionMemory;

namespace ABHeal
{
    public class PEntity : Entity
    {
        public bool active = false;
        public PType type;
        public DateTime bBOHTime;
        public DateTime bBORTime;

        public PEntity(uint aPtr, string aType) : base((int)aPtr)
        {
            
            

            switch (aType)
            {
                case "MainTank":
                    type = PType.MainTank;
                    break;
                case "Warrior":
                    type = PType.Warrior;
                    break;
                case "Healer":
                    type = PType.Healer;
                    break;
                case "Scout":
                    type = PType.Scout;
                    break;
                default:
                    type = PType.Mage;
                    break;
            }

            TimeSpan ts = TimeSpan.FromMinutes(60);
            bBOHTime = DateTime.Now.Subtract(ts);
            bBORTime = DateTime.Now.Subtract(ts);
            
        }
    }
}
