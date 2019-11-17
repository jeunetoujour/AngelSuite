/*
    Copyright © 2009, AionHacker.net
    All rights reserved.
    http://www.aionhacker.net
    http://www.assembla.com/spaces/AionMemory


    This file is part of AionMemory.

    AionMemory is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    AionMemory is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with AionMemory.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AionMemory
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
