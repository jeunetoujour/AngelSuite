using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABHeal
{
    public enum RadiantCure { Cast = 3000, Cool = 6000 }
    public enum HealingLight { Cast = 2000, Cool = 100 }
    public enum FlashofRecovery { Cast = 600, Cool = 30000 }
    public enum LightofRecovery { Cast = 1000, Cool = 2000 }

    public enum BlessingOfHealth { Cast = 1000, Cool = 1000, Duration = 3600 }
    public enum BlessingOfRock { Cast = 1000, Cool = 1000, Duration = 3600 }

    public enum PType { MainTank, Warrior, Healer, Scout, Mage }
}
