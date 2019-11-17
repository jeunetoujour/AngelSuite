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

namespace Eve
{
    public class LootEventArgs : EventArgs
    {
        private string[] _Items;
        private string _Who;

        public string Who
        {
            get { return _Who; }
        }

        public string[] Items
        {
            get { return _Items; }
        }

        internal LootEventArgs(string who, string[] items)
        {
            _Who = who;
            _Items = items;
        }
    }

    public class DeathEventArgs : EventArgs
    {
        internal DeathEventArgs()
        {
            
        }
    }

    public class ChatEventArgs : EventArgs
    {
        private string _Who;
        private string _Channel;
        private string _Message;
        private string[] _MessageArray;

        public string Who
        {
            get { return _Who; }
        }

        public string Channel
        {
            get { return _Channel; }
        }

        public string Message
        {
            get { return _Message; }
        }

        public string[] MessageArray
        {
            get { return _MessageArray; }
        }

        internal ChatEventArgs(string who, string channel, string message)
        {
            _Who = who;
            _Channel = channel;
            _Message = message;
            _MessageArray = message.Split(' ');
        }
    }

    public class CubeEventArgs : EventArgs
    {
        private string[] _Items;
        private string _Who;

        public string Who
        {
            get { return _Who; }
        }

        public string[] Items
        {
            get { return _Items; }
        }

        internal CubeEventArgs(string who, string[] items)
        {
            _Who = who;
            _Items = items;
        }
    }
}
