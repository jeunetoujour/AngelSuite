using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>Entity attitude enum.</summary>
    public enum eAttitude
    {
        Passive = 0,
        Hostile = 8,
        Friendly = 38,
        Utility = 294
    }

    /// <summary>Entity class enum.</summary>
    public enum eClass
    {
        Warrior = 0,
        Gladiator,
        Templar,
        Scout,
        Assassin,
        Ranger,
        Mage,
        Sorcerer,
        Spiritmaster,
        Priest,
        Cleric,
        Chanter
    }

    
    /// <summary>Entity stance enum.</summary>
    public enum eStance
    {
        Normal = 0,
        Combat = 1,
        Resting = 3,
        Flying = 4,
        FlyingCombat = 5,
        Dead = 7
    }

    /// <summary>Entity type enum.</summary>
    public enum eType
    {
        Player = 0,
        Object = 7,
        AttackableNPC = 12,
        FriendlyNPC = 14,
        Vendor = 16,
        DeadwLoot = 36,
        DeadnoLoot = 37,
        Gatherable = 39
    }

    /// <summary>Entity quest enum.</summary>
    public enum eQuest
    {
        None = 0,
        NotAvailable,
        New = 3,
        InProgress = 7,
        Complete = 11
    }
}
