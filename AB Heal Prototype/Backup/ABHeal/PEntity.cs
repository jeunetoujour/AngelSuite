namespace ABHeal
{
    using AionMemory;
    using System;

    public class PEntity : Entity
    {
        public bool active;
        public DateTime bBOHTime;
        public DateTime bBORTime;
        public PType type;

        public PEntity(uint aPtr, string aType) : base((int) aPtr)
        {
            TimeSpan span;
            string str = aType;
            if (str != null)
            {
                if (!(str == "MainTank"))
                {
                    if (str == "Warrior")
                    {
                        this.type = PType.Warrior;
                        goto Label_006D;
                    }
                    if (str == "Healer")
                    {
                        this.type = PType.Healer;
                        goto Label_006D;
                    }
                    if (str == "Scout")
                    {
                        this.type = PType.Scout;
                        goto Label_006D;
                    }
                }
                else
                {
                    this.type = PType.MainTank;
                    goto Label_006D;
                }
            }
            this.type = PType.Mage;
        Label_006D:
            span = TimeSpan.FromMinutes(60.0);
            this.bBOHTime = DateTime.Now.Subtract(span);
            this.bBORTime = DateTime.Now.Subtract(span);
        }
    }
}

