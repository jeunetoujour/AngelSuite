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

    public enum nExpertise
    {
        Novice,
        Disciplined,
        Seasoned,
        Expert,
        Veteran,
        Master,
        Unk = -1

    }

    public enum nType
    {
        Normal,
        Elite,
        Hero,
        Legendary,
        Unk = -1
    }

    /// <summary>Entity stance enum.</summary>
    public enum eStance
    {
        Normal = 0,
        Combat = 1,
        Gliding = 2,
        Resting = 3,
        Flying = 4,
        FlyingCombat = 5,
        GlidingFlyArea = 6,
        Dead = 7
    }
    /*
     * 1=normal/reseting
33=mobattacking
5=resting
7=dead
65=walking
44=looting
3=flying
0=nothing
2=occupied
     * 
     * */
    /// <summary>Entity type enum.</summary>
    public enum eType
    {
        Player = 0,
        Player1 = 1,
        NPC = 2,
        Rift = 3,
        Kisk = 4,
        Object = 7,
        Place = 10,
        AttackableNPC = 12,
        EnemyPlayer = 13,
        FriendlyNPC = 14,
        Vendor = 16,
        GatherableL = 20,
        DeadwLoot = 36,
        DeadnoLoot = 37,
        Gatherable = 39,
        GatherableNoSkill = 40
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
