/*
    Copyright © 2009, AionHacker.net
    All rights reserved.
    http://www.aionhacker.net
    http://www.assembla.com/spaces/AionMemory


    This file is part of Eve.

    Eve is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Eve is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Eve.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AionMemory;
using MemoryLib;

namespace Eve
{
    public class Eve
    {
        public Target TargetNearestMob()
        {
            using (Target target = new Target())
            {
                return target;
            }
        }

        public void UseSkill(string skill)
        {

        }

        public void UseQuickslot(string slot)
        {

        }

        public void UseItem(string item)
        {

        }

        public void Return()
        {

        }

        public void Loot()
        {
            using (Target target = new Target())
            {
                OnLoot(this, new LootEventArgs(target.Name, new string[2] { "item1", "item2" }));
            }
        }

        #region Events

        public event LootHandler OnLoot;
        public event DeathHandler OnDeath;
        public event PrivMsgHandler OnPrivMsg;
        public event CubeFullHandler OnCubeFull;

        #endregion

        #region Event Delegates

        public delegate void LootHandler(object sender, LootEventArgs e);
        public delegate void DeathHandler(object sender, DeathEventArgs e);
        public delegate void PrivMsgHandler(object sender, ChatEventArgs e);
        public delegate void CubeFullHandler(object sender, CubeEventArgs e);

        #endregion
    }
}
