/*
    Copyright © 2009, Aion-Radar.com
    All rights reserved.
    http://www.aion-radar.com


    This file is part of Aion Radar.

    Aion Radar is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Aion Radar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Aion Radar.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public partial class UpdateProgress : Form
{
    public UpdateProgress()
    {
        InitializeComponent();
    }

    public int Progress
    {
        get
        {
            return this.progressBar.Value;
        }
        set
        {
            this.progressBar.Value = value;
            Application.DoEvents();
        }
    }

    public string Status
    {
        get
        {
            return this.Text;
        }
        set
        {
            this.Text = "Auto Update - " + value;
            Application.DoEvents();
        }
    }
}
