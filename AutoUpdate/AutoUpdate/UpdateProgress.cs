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
