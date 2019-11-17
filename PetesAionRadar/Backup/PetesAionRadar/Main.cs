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
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using AionMemory;

namespace PetesAionRadar
{
    public partial class frmMain : Form
    {
        private EntityList elist = new EntityList();
        private Player player = null;//new Player();
        //private Party party = null;

        private double dZoomLevel = 100.0;
        private bool bOverrideStickOn = false;
        private bool bCompassMode = false;
        private bool bBranding = true;

        private Color TRColor = Color.FromArgb(60, 60, 60);

        private Font fTextNormal = new Font("Arial", 8, FontStyle.Regular);
        private Font fTextBranding = new Font("Arial", 11, FontStyle.Bold);
        private Font fTextImportant = new Font("Tahoma", 10, FontStyle.Bold | FontStyle.Italic);

        private Dictionary<string, Brush> brushes = new Dictionary<string, Brush>();
        private Dictionary<string, Icon> icons = new Dictionary<string, Icon>();
        private Dictionary<string, Bitmap> classIcons = new Dictionary<string, Bitmap>();

        private Brush bTextWhite = new System.Drawing.SolidBrush(Color.White);
        private Brush bTextBlack = new System.Drawing.SolidBrush(Color.Black);
        private Brush bTextShadow = new System.Drawing.SolidBrush(Color.FromArgb(67, 67, 67));
        private Brush bTextShadowFire = new System.Drawing.SolidBrush(Color.FromArgb(255, 0, 0));
        private Brush bTextShadowNuclear = new System.Drawing.SolidBrush(Color.FromArgb(0, 255, 0));
        private Brush bTextShadowAura = new System.Drawing.SolidBrush(Color.FromArgb(170, 0, 255));
        private Brush bTextShadowIce = new System.Drawing.SolidBrush(Color.FromArgb(170, 211, 255));

        private Bitmap CompassRing = null;
        private Pen circlePen = null;

        public frmMain()
        {
            this.InitializeComponent();
            
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.TopMost = true;
            this.Hide();
            this.picRadar.BorderStyle = BorderStyle.None;
        }

        private void menuMain_LayoutCompleted(object sender, EventArgs e)
        {
            this.LoadSettings();
        }

        /// <summary>
        /// Checks for the AION game client window resizing and adjusts accordingly if
        /// we aren't in Compass Mode.  This could be improved by hooking the resize
        /// message events from the AION game client window.
        /// </summary>
        private void EnsureWindowSizeIsCorrect()
        {
            if (this.bCompassMode)
                return;

            IntPtr hWndAion = Utils.FindWindow(null, "AION Client");
            
            if (hWndAion != IntPtr.Zero)
            {
                if (!Utils.IsZoomed(hWndAion))
                {
                    Utils.RECT r = new Utils.RECT();
                    Utils.GetWindowRect(hWndAion, ref r);
                    this.Top = r.Top + 28;
                    this.Left = r.Left + 6;
                    this.Width = r.Right - r.Left - 11;
                    this.Height = r.Bottom - r.Top - 34;
                    return;
                }
            }

            Rectangle rBounds = Screen.GetWorkingArea(this.picRadar);
            this.Top = rBounds.Top + 22;
            this.Left = rBounds.Left;
            this.Width = rBounds.Width;
            this.Height = rBounds.Height - 22;
        }

        /// <summary>
        /// Allows dragging the window around by the compass ring as well as showing
        /// the context menu if the compass ring is right-clicked.
        /// </summary>
        void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (!this.bCompassMode)
                return;

            if (e.Button == MouseButtons.Left)
            {
                Utils.ReleaseCapture();
                Utils.SendMessage(Handle, Utils.WM_NCLBUTTONDOWN, Utils.HTCAPTION, 0);
            }

            if (e.Button == MouseButtons.Right)
                this.menuMain.Show((Control)sender, e.X, e.Y);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.EnsureWindowSizeIsCorrect();

            this.LoadSettings();

            this.MouseDown += new MouseEventHandler(Form_MouseDown);
            this.picRadar.MouseDown += new MouseEventHandler(Form_MouseDown);  

            this.brushes.Add("Player", Utils.BrushColor("42D16C"));
            this.brushes.Add("PlayerDead", Utils.BrushColor("42D16C"));
            this.brushes.Add("PlayerFriendly", Utils.BrushColor("42D16C"));
            this.brushes.Add("PlayerHostile", Utils.BrushColor("FF7070"));
            this.brushes.Add("PlayerGroup", Utils.BrushColor("80D0D0"));
            this.brushes.Add("PlayerGroupDead", Utils.BrushColor("80A0A0"));
            this.brushes.Add("NPCDead", Utils.BrushColor("A0A0A0"));
            this.brushes.Add("NPCLoot", Utils.BrushColor("ffE750"));
            this.brushes.Add("NPC", Utils.BrushColor("ffffff"));
            this.brushes.Add("NPCAttackable", Utils.BrushColor("d7d750"));
            this.brushes.Add("NPCFriendly", Utils.BrushColor("ffffff"));
            this.brushes.Add("NPCHostile", Utils.BrushColor("ff3030"));
            this.brushes.Add("NPCHostileLow", Utils.BrushColor("A0A0A0"));
            this.brushes.Add("Object", Utils.BrushColor("A0A0A0"));
            this.brushes.Add("Landmark", Utils.BrushColor("C787ff"));
            this.brushes.Add("Gatherable", Utils.BrushColor("B0E060"));
            this.brushes.Add("GatherableAir", Utils.BrushColor("60B0E0"));
            this.brushes.Add("Vendor", Utils.BrushColor("A0A0E0"));
            this.brushes.Add("Unknown", Utils.BrushColor("D0ffF0"));

            this.icons.Add("Player", Utils.LoadIconFromResource("resources.playerfriendly.ico"));
            this.icons.Add("PlayerDead", Utils.LoadIconFromResource("resources.playerdead.ico"));
            this.icons.Add("PlayerFriendly", Utils.LoadIconFromResource("resources.playerfriendly.ico"));
            this.icons.Add("PlayerHostile", Utils.LoadIconFromResource("resources.playerhostile.ico"));
            this.icons.Add("PlayerGroup", Utils.LoadIconFromResource("resources.playergroup.ico"));
            this.icons.Add("PlayerGroupDead", Utils.LoadIconFromResource("resources.playergroupdead.ico"));
            this.icons.Add("NPCDead", Utils.LoadIconFromResource("resources.npcdead.ico"));
            this.icons.Add("NPCLoot", Utils.LoadIconFromResource("resources.npcloot.ico"));
            this.icons.Add("NPC", Utils.LoadIconFromResource("resources.npcsmall.ico"));
            this.icons.Add("NPCAttackable", Utils.LoadIconFromResource("resources.npc.ico"));
            this.icons.Add("NPCFriendly", Utils.LoadIconFromResource("resources.npcfriendly.ico"));
            this.icons.Add("NPCHostile", Utils.LoadIconFromResource("resources.npchostile.ico"));
            this.icons.Add("NPCHostileLow", Utils.LoadIconFromResource("resources.npchostile.ico"));
            this.icons.Add("Object", Utils.LoadIconFromResource("resources.object.ico"));
            this.icons.Add("Landmark", Utils.LoadIconFromResource("resources.landmark.ico"));
            this.icons.Add("Gatherable", Utils.LoadIconFromResource("resources.gather.ico"));
            this.icons.Add("GatherableAir", Utils.LoadIconFromResource("resources.gatherair.ico"));
            this.icons.Add("Vendor", Utils.LoadIconFromResource("resources.vendor.ico"));
            this.icons.Add("Unknown", Utils.LoadIconFromResource("resources.unknown.ico"));

            this.classIcons.Add("Assassin", Utils.LoadBitmapFromResource("resources.Assassin.bmp"));
            this.classIcons.Add("Chanter", Utils.LoadBitmapFromResource("resources.Chanter.bmp"));
            this.classIcons.Add("Cleric", Utils.LoadBitmapFromResource("resources.Cleric.bmp"));
            this.classIcons.Add("Gladiator", Utils.LoadBitmapFromResource("resources.Gladiator.bmp"));
            this.classIcons.Add("Ranger", Utils.LoadBitmapFromResource("resources.Ranger.bmp"));
            this.classIcons.Add("Sorcerer", Utils.LoadBitmapFromResource("resources.Sorcerer.bmp"));
            this.classIcons.Add("Spiritmaster", Utils.LoadBitmapFromResource("resources.Spiritmaster.bmp"));
            this.classIcons.Add("Templar", Utils.LoadBitmapFromResource("resources.Templar.bmp"));

            this.CompassRing = Utils.LoadBitmapFromResource("resources.compass_ring.bmp");

            if (this.bCompassMode)
            {
                this.Width = 225;
                this.Height = 225;
            }

            this.circlePen = new Pen(System.Drawing.Color.DarkBlue, 1);
            this.TransparencyKey = Color.FromArgb(0, 0, 0);
            this.TransparencyKey = this.TRColor;
            this.BackColor = this.TRColor;
            this.picRadar.BackColor = this.TRColor;
            this.picRadar.CreateGraphics().Clear(this.TRColor);

            this.Show();

            this.TransparencyKey = Color.FromArgb(0, 0, 0);
            this.TransparencyKey = this.TRColor;
            this.Opacity = 0.0;
            this.Opacity = 1.0;

            this.tmrRadarUpdate.Enabled = true;

            Utils.SetForegroundWindow(Utils.FindWindow(null, "AION Client"));
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.SaveSettings();
        }

        private void LoadSettings()
        {
            if (!string.IsNullOrEmpty(Utils.GetAppSetting("TransparencyColor")))
                this.TRColor = Utils.HexColor(Utils.GetAppSetting("TransparencyColor"));

            this.playersToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowPlayers").Trim().ToLower() == "true");
            this.playershostileToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowPlayersHostile").Trim().ToLower() != "false");
            this.playersgroupToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowPlayersGroup").Trim().ToLower() != "false");
            this.nPCsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCs").Trim().ToLower() == "true");
            this.nPCspeacefulToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCsPeaceful").Trim().ToLower() == "true");
            this.nPCshostileToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCsHostile").Trim().ToLower() != "false");
            this.nPCshostilelowToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCsHostileLow").Trim().ToLower() == "true");
            this.nPCsdeadToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCsDead").Trim().ToLower() != "false");
            this.nPCsotherToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowNPCsOther").Trim().ToLower() != "false");
            this.gatherableToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowGatherable").Trim().ToLower() != "false");
            this.gatherableairToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowGatherableAir").Trim().ToLower() != "false");
            this.objectsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowObjects").Trim().ToLower() != "false");
            this.vendorsShopsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowVendors").Trim().ToLower() != "false");
            this.landmarksToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowLandmarks").Trim().ToLower() != "false");
            this.showCrosshairToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowCrosshair").Trim().ToLower() != "false");
            this.showSelfToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowSelf").Trim().ToLower() == "true");
            this.showLevelsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowLevels").Trim().ToLower() != "false");
            this.showLegionsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowLegions").Trim().ToLower() == "true");
            this.showHealthToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowHealth").Trim().ToLower() != "false");
            this.showUndergroundTargetsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowUndergroundTargets").Trim().ToLower() != "false");
            this.onlyDisplayRadarWhenAionIsTheActiveApplicationToolStripMenuItem.Checked = (Utils.GetAppSetting("OnlyDisplayRadarWhenAionIsActive").Trim().ToLower() != "false");
            this.useHighestQualityRenderingToolStripMenuItem.Checked = (Utils.GetAppSetting("UseHighQualityRendering").Trim().ToLower() == "true");
            this.showDescriptionsToolStripMenuItem.Checked = (Utils.GetAppSetting("ShowDescriptions").Trim().ToLower() != "false");
            this.bCompassMode = this.compassModeToolStripMenuItem.Checked = (Utils.GetAppSetting("CompassMode").Trim().ToLower() == "true");
            this.classIconsForPlayersToolStripMenuItem.Checked = (Utils.GetAppSetting("ClassIcons").Trim().ToLower() != "false");

            this.toolStripMenuItem9.Checked = false;
            this.toolStripMenuItem13.Checked = false;
            this.toolStripMenuItem10.Checked = false;
            this.toolStripMenuItem11.Checked = false;
            this.toolStripMenuItem12.Checked = false;
            if (Utils.GetAppSetting("ZoomLevel").Trim() == "150")
            {
                this.dZoomLevel = 150.0;
                this.toolStripMenuItem13.Checked = true;
            }
            else if (Utils.GetAppSetting("ZoomLevel").Trim() == "200")
            {
                this.dZoomLevel = 200.0;
                this.toolStripMenuItem10.Checked = true;
            }
            else if (Utils.GetAppSetting("ZoomLevel").Trim() == "300")
            {
                this.dZoomLevel = 300.0;
                this.toolStripMenuItem11.Checked = true;
            }
            else if (Utils.GetAppSetting("ZoomLevel").Trim() == "400")
            {
                this.dZoomLevel = 400.0;
                this.toolStripMenuItem12.Checked = true;
            }
            else
            {
                this.dZoomLevel = 100.0;
                this.toolStripMenuItem9.Checked = true;
            }
        }

        private void SaveSettings()
        {
            Utils.SetAppSetting("ShowPlayers", this.playersToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowPlayersHostile", this.playershostileToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowPlayersGroup", this.playersgroupToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCs", this.nPCsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCsPeaceful", this.nPCspeacefulToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCsHostile", this.nPCshostileToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCsHostileLow", this.nPCshostilelowToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCsDead", this.nPCsdeadToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowNPCsOther", this.nPCsotherToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowGatherable", this.gatherableToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowGatherableAir", this.gatherableairToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowObjects", this.objectsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowVendors", this.vendorsShopsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowLandmarks", this.landmarksToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowCrosshair", this.showCrosshairToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowSelf", this.showSelfToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowLevels", this.showLevelsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowLegions", this.showLegionsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowHealth", this.showHealthToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowUndergroundTargets", this.showUndergroundTargetsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("OnlyDisplayRadarWhenAionIsActive", this.onlyDisplayRadarWhenAionIsTheActiveApplicationToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("UseHighQualityRendering", this.useHighestQualityRenderingToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ShowDescriptions", this.showDescriptionsToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("CompassMode", this.compassModeToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ClassIcons", this.classIconsForPlayersToolStripMenuItem.Checked.ToString());
            Utils.SetAppSetting("ZoomLevel", this.dZoomLevel.ToString("0"));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tmrRadarUpdate.Enabled = false;
            this.SaveSettings();
            Application.Exit();
        }

        private void menuMain_Opening(object sender, CancelEventArgs e)
        {
            this.bOverrideStickOn = true;
        }

        private void menuMain_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            bOverrideStickOn = false;
            Utils.SetForegroundWindow(Utils.FindWindow(null, "AION Client"));
        }

        /// <summary>
        /// There's some extra logic in here so that the application doesn't crash if
        /// the Aion Memory libraries haven't loaded yet.  Aion Memory should fix this
        /// internally so that loading the library without the AION Client game window
        /// present doesn't throw an exception.
        /// </summary>
        private void tmrRadarUpdate_Tick(object sender, EventArgs e)
        {
            this.tmrRadarUpdate.Enabled = false;

            this.EnsureWindowSizeIsCorrect();

            try
            {
                if (AionMemory.Process.handle == IntPtr.Zero)
                    AionMemory.Process.Open();
                if (AionMemory.Process.handle == IntPtr.Zero)
                    this.tmrRadarUpdate.Interval = 50;
                else
                {
                    this.tmrRadarUpdate.Interval = 50;
                    player = new AionMemory.Player();
                }
            }
            catch { }

            try
            {
                if (AionMemory.Process.handle != IntPtr.Zero)
                {
                    // Get an updated list of entities and player data.
                    this.elist.Update();
                    this.player.Updatenamelvl();
                    this.player.UpdateRot();
                    this.player.Update();
                }
            }
            catch { }

            try { this.DrawRadar(); } catch { }

            this.tmrRadarUpdate.Enabled = true;
        }

        private void DrawRadar()
        {
            // Don't draw the radar if the game or radar isn't actively in focus
            if (onlyDisplayRadarWhenAionIsTheActiveApplicationToolStripMenuItem.Checked && !this.bOverrideStickOn)
            {
                if ((Utils.GetActiveWindowTitle() != "AION Client") && (Utils.GetForegroundWindow() != this.Handle))
                {
                    this.picRadar.CreateGraphics().Clear(this.TRColor);
                    return;
                }
            }

            // Try to update the list of group members
            try
            {
                if (AionMemory.Process.handle != IntPtr.Zero)
                {
                    //this.party = new Party();
                    //party.Update();
                }
            }
            catch { }

            // Create an offscreen rendering plane to draw to (reduces flickering)
            Bitmap offScreenBmp = new Bitmap(this.picRadar.Width, this.picRadar.Height);
            Graphics offScreenDC = Graphics.FromImage(offScreenBmp);

            // Use highest quality rendering upon request
            if (useHighestQualityRenderingToolStripMenuItem.Checked)
            {
                offScreenDC.SmoothingMode = SmoothingMode.HighQuality;
                offScreenDC.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                offScreenDC.CompositingMode = CompositingMode.SourceOver;
                offScreenDC.CompositingQuality = CompositingQuality.HighQuality;
            }

            // Wipe the offscreen renderer with our transparency key
            offScreenDC.Clear(this.TRColor);

            // Set up our bounds as appropriate
            int centerX = picRadar.Width / 2;
            int centerY = picRadar.Height / 2;
            double maxRadius = (Math.Min(picRadar.Height, picRadar.Width) - 20) / 2;

            // Draw the compass frame if necessary
            if (this.bCompassMode)
                offScreenDC.DrawImage(this.CompassRing, new Point(0, 0));

            if (this.bBranding)
            {
                // Get version information
                System.Diagnostics.FileVersionInfo fv;
                fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

                if (this.bCompassMode)
                {
                    //OutputString(ref offScreenDC, Application.ProductName.Replace(" ", "  ") + "  v." + fv.FileMajorPart + "." + Convert.ToInt32(fv.FileMinorPart).ToString("00"), fTextImportant, bTextWhite, bTextShadow, new Point(this.picRadar.Width - 195, 0));
                    //OutputString(ref offScreenDC, "PUBLIC BETA TEST VERSION", fTextNormal, bTextWhite, bTextShadowFire, new Point(this.picRadar.Width - 190, this.picRadar.Height - 14));
                    //OutputString(ref offScreenDC, "TEST RELEASE: DO NOT COPY!", fTextNormal, bTextWhite, bTextShadowFire, new Point(this.picRadar.Width - 190, this.picRadar.Height - 14));
                }
                else
                {
                    //OutputString(ref offScreenDC, Application.ProductName.Replace(" ", "  ") + "  v." + fv.FileMajorPart + "." + Convert.ToInt32(fv.FileMinorPart).ToString("00"), fTextNormal, bTextWhite, bTextShadow, new Point(4, 4));
                    //OutputString(ref offScreenDC, "Developed  for  Aion  Online  v.1.5.0.11.", fTextNormal, bTextWhite, bTextShadow, new Point(4, 22));
                    //OutputString(ref offScreenDC, "With  thanks  to  the  Aion Memory  project.", fTextNormal, bTextWhite, bTextShadow, new Point(4, 36));
                    //OutputString(ref offScreenDC, "PUBLIC  BETA  TEST  VERSION,  ENJOY  AND  REPORT", fTextBranding, bTextWhite, bTextShadowFire, new Point((this.picRadar.Width / 2) - 180, 25));
                    //OutputString(ref offScreenDC, "BUGS  AND  SUGGESTIONS  TO:   AionRadar@gmail.com", fTextBranding, bTextWhite, bTextShadowFire, new Point((this.picRadar.Width / 2) - 188, 40));
                    //OutputString(ref offScreenDC, "THIS IS A TEST / DEBUG RELEASE AND SHOULD NOT BE DISTRIBUTED!", fTextImportant, bTextWhite, bTextShadowFire, new Point((this.picRadar.Width / 2) - 220, 25));
                }
            }

            // Draw distance circles
            /*
            offScreenDC.DrawEllipse(circlePen,
                (int)(centerX - maxRadius/4), (int)(centerY - maxRadius/4),
                (int)(2 * maxRadius/4), (int)(2 * maxRadius/4));
            offScreenDC.DrawEllipse(circlePen,
                (int)(centerX - maxRadius/2), (int)(centerY - maxRadius/2),
                (int)(2 * maxRadius/2), (int)(2 * maxRadius/2));
            offScreenDC.DrawEllipse(circlePen,
                (int)(centerX - maxRadius * 3 / 4), (int)(centerY - maxRadius * 3 / 4),
                (int)(2 * maxRadius*3/4), (int)(2 * maxRadius*3/4));
            offScreenDC.DrawEllipse(circlePen,
                (int)(centerX - maxRadius), (int)(centerY - maxRadius),
                (int)(2 * maxRadius), (int)(2 * maxRadius));
            */

            // This converts AION's representation of rotation degrees to real world data.
            double dRealRot = 0;
            try { dRealRot = -player.Rotation; } catch { } // this will throw an exception if AION Memory hasn't loaded yet
            if (dRealRot < 0) dRealRot += 360;
            if (dRealRot >= 360) dRealRot -= 360;
            int NewCenterX = 0, NewCenterY = 0;
            try { this.LocConverter(player.X, player.Y, player.X, player.Y, centerX, centerY, dRealRot, out NewCenterX, out NewCenterY); } catch { }

            // Draw the crosshair
            if (showCrosshairToolStripMenuItem.Checked)
            {
                offScreenDC.DrawLine(circlePen, new Point(centerX - 3, centerY), new Point(centerX + 3, centerY));
                offScreenDC.DrawLine(circlePen, new Point(centerX, centerY - 3), new Point(centerX, centerY + 3));
            }

            // This first loop draws class icons.  Since they're so big, we want them
            // in the background, thus drawn first underneath everything else to come...
            if (this.classIconsForPlayersToolStripMenuItem.Checked && this.showHealthToolStripMenuItem.Checked && !this.bCompassMode)
            {
                try
                {
                    foreach (var entity in elist)
                    {
                        try
                        {
                            int satX = 0, satY = 0;
                            this.LocConverter(entity.X, entity.Y, player.X, player.Y, NewCenterX, NewCenterY, dRealRot, out satX, out satY);

                            // Don't show self
                            if ((entity.ID == player.ID) && (!this.showSelfToolStripMenuItem.Checked))
                                continue;

                            // Don't show targets under the floor
                            if (
                                ((entity.Z < (player.Z - 20.0)) && (player.Stance != eStance.Flying) && (player.Stance != eStance.FlyingCombat))
                                 && (!this.showUndergroundTargetsToolStripMenuItem.Checked)
                                )
                                continue;

                            // Don't show unnamed targets
                            if (string.IsNullOrEmpty(entity.Name)) continue;
                            if (entity.Name.Trim().Length < 1) continue;

                            string sType = entity.Type.ToString();

                            if (sType == "Collectable")
                                sType = "Gatherable";
                            else if (sType == "Gatherable")
                                sType = "Gatherable";
                            else if (sType == "FriendlyNPC")
                                sType = "NPCFriendly";
                            else if ((sType == "Gatherable") && (entity.Name.Contains("Vortex") || entity.Name.Contains("Aether")))
                                sType = "GatherableAir";
                            else if (sType == "ObjectClickable")
                                sType = "Object";

                            if (sType == "AttackableNPC")
                            {
                                sType = "NPCAttackable";
                                if (entity.Attitude == eAttitude.Hostile)
                                {
                                    sType = "NPCHostile";
                                    if (entity.Level + 10 <= player.Level)
                                        sType = "NPCHostileLow";
                                }
                                else if ((entity.IsDead) && (entity.Health <= 0))
                                    sType = "NPCDead";

                                if ((entity.Attitude == eAttitude.Friendly) || (entity.Class != eClass.Warrior))
                                    sType = "PlayerHostile";
                            }
                            else if ((sType == "Player") || (sType == "PlayerHostile"))
                            {
                                if (entity.Attitude == eAttitude.Hostile)
                                    sType = "PlayerHostile";

                                if (entity.Level == 0) // landmark
                                    sType = "Landmark";
                                else if ((entity.IsDead) && (entity.Health <= 0))
                                    if ((entity.Attitude != eAttitude.Friendly) && (entity.Attitude != eAttitude.Hostile))
                                        sType = "Unknown";
                            }

                            if (sType == "Player")
                            {
                                try
                                {
                                    foreach (var p in party.Member)
                                        if (p.ID == entity.ID)
                                            sType = "PlayerGroup";
                                }
                                catch { }
                            }

                            if (sType == "PlayerDead")
                            {
                                try
                                {
                                    foreach (var p in party.Member)
                                        if (p.ID == entity.ID)
                                            sType = "PlayerGroupDead";
                                }
                                catch { }
                            }

                            if ((sType == "Player") && (player.ID != entity.ID) && (!this.playersToolStripMenuItem.Checked)) continue;
                            if ((sType == "PlayerDead") && (!this.playersToolStripMenuItem.Checked)) continue;
                            if ((sType == "PlayerHostile") && (!this.playershostileToolStripMenuItem.Checked)) continue;
                            if ((sType == "PlayerGroup") && (!this.playersgroupToolStripMenuItem.Checked)) continue;
                            if ((sType == "PlayerGroupDead") && (!this.playersgroupToolStripMenuItem.Checked)) continue;

                            if ((sType == "NPC") && (!this.nPCsotherToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCDead") && (!this.nPCsdeadToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCLoot") && (!this.nPCsdeadToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCFriendly") && (!this.nPCsToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCAttackable") && (!this.nPCspeacefulToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCHostile") && (!this.nPCshostileToolStripMenuItem.Checked)) continue;
                            if ((sType == "NPCHostileLow") && (!this.nPCshostilelowToolStripMenuItem.Checked)) continue;

                            if ((sType == "Gatherable") && (!this.gatherableToolStripMenuItem.Checked)) continue;
                            if ((sType == "GatherableAir") && (!this.gatherableairToolStripMenuItem.Checked)) continue;
                            if ((sType == "Object") && (!this.objectsToolStripMenuItem.Checked)) continue;
                            if ((sType == "Vendor") && (!this.vendorsShopsToolStripMenuItem.Checked)) continue;
                            if ((sType == "Landmark") && (!this.landmarksToolStripMenuItem.Checked || this.bCompassMode)) continue;

                            if (sType.StartsWith("Player"))
                                offScreenDC.DrawImage(this.classIcons[entity.Class.ToString()], satX - (this.classIcons[entity.Class.ToString()].Width - 2), satY + 3, this.classIcons[entity.Class.ToString()].Width, this.classIcons[entity.Class.ToString()].Height);
                        }
                        catch { }
                    }
                }
                catch { }
            }

            // This second loop draws icons, text, health bars, etc.
            try
            {
                foreach (var entity in elist)
                {
                    try
                    {
                        int satX = 0, satY = 0;
                        try
                        {
                            this.LocConverter(entity.X, entity.Y, player.X, player.Y, NewCenterX, NewCenterY, dRealRot, out satX, out satY);
                        }
                        catch { }

                        // don't show self
                        if ((entity.ID == player.ID) && (!this.showSelfToolStripMenuItem.Checked))
                            continue;

                        // don't show targets under the floor
                        if (
                            ((entity.Z < (player.Z - 20.0)) && (player.Stance != eStance.Flying) && (player.Stance != eStance.FlyingCombat))
                             && (!this.showUndergroundTargetsToolStripMenuItem.Checked)
                            )
                            continue;

                        // don't show unnamed targets
                        if (string.IsNullOrEmpty(entity.Name)) continue;
                        if (entity.Name.Trim().Length < 1) continue;

                        string sType = entity.Type.ToString();
                        string sDescOutput = entity.Name;

                        if (sType == "Collectable")
                            sType = "Gatherable";
                        else if (sType == "GatherableNoSkill" || sType == "40")
                            sType = "Gatherable";
                        else if (sType == "FriendlyNPC")
                            sType = "NPCFriendly";
                        else if ((sType == "Gatherable") && (entity.Name.Contains("Vortex") || entity.Name.Contains("Aether")))
                            sType = "GatherableAir";
                        else if (sType == "ObjectClickable")
                            sType = "Object";

                        string sLevelInfo = string.Empty;
                        if (this.showLevelsToolStripMenuItem.Checked)
                            sLevelInfo = " (" + entity.Level.ToString() + ")";

                        if (sType == "AttackableNPC")
                        {
                            sType = "NPCAttackable";
                            if (entity.Attitude == eAttitude.Hostile)
                            {
                                sType = "NPCHostile";
                                if (entity.Level + 10 <= player.Level)
                                    sType = "NPCHostileLow";
                            }
                            if (entity.Health == 100)
                                sDescOutput = entity.Name + sLevelInfo;
                            else if ((!entity.IsDead) || (entity.Health > 0))
                                sDescOutput = entity.Name + sLevelInfo;
                            else
                            {
                                sDescOutput = entity.Name + sLevelInfo;
                                sType = "NPCDead";
                            }

                            if ((entity.Attitude == eAttitude.Friendly) || (entity.Class != eClass.Warrior))
                                sType = "PlayerHostile";
                        }
                        else if (sType == "NPC")
                            sDescOutput = string.Empty;
                        else if (sType == "NPCDead")
                            sDescOutput = entity.Name;
                        else if (sType == "NPCLoot")
                            sDescOutput = entity.Name;
                        else if (sType == "NPCFriendly")
                            sDescOutput = entity.Name;
                        else if (sType == "Gatherable")
                        {
                            sDescOutput = entity.Name;
                            if (entity.Type == eType.Gatherable)
                                sDescOutput += " (no skill)";
                        }
                        else if (sType == "GatherableAir")
                            sDescOutput = entity.Name;
                        else if (sType == "Object")
                            sDescOutput = entity.Name;
                        else if ((sType == "Player") || (sType == "PlayerHostile"))
                        {
                            if (entity.Attitude == eAttitude.Hostile)
                                sType = "PlayerHostile";

                            if (entity.Level == 0) // landmark
                                sType = "Landmark";
                            else if (entity.Health == 100)
                            {
                                //if (string.IsNullOrEmpty(entity.Legion) || (!this.showLegionsToolStripMenuItem.Checked))
                                    sDescOutput = entity.Name + sLevelInfo;
                               // else
                                   // sDescOutput = entity.Name + " (" + entity.Legion + ")" + sLevelInfo;
                            }
                            else if ((!entity.IsDead) || (entity.Health > 0))
                                sDescOutput = entity.Name + sLevelInfo;
                            else
                            {
                                if ((entity.Attitude == eAttitude.Friendly) || (entity.Attitude == eAttitude.Hostile))
                                    sDescOutput = entity.Name + sLevelInfo;
                                else
                                {
                                    sDescOutput = entity.Name;
                                    sType = "Unknown";
                                }
                            }
                        }
                        else if (sType == "Vendor")
                            sDescOutput = entity.Name;
                        else
                            sDescOutput = entity.Name + " (unknown: " + entity.Type.ToString() + ")";

                        if (sType == "Player")
                        {
                            try
                            {
                                foreach (var p in party.Member)
                                    if (p.ID == entity.ID)
                                        sType = "PlayerGroup";
                            }
                            catch { }
                        }

                        if (sType == "PlayerDead")
                        {
                            try
                            {
                                foreach (var p in party.Member)
                                    if (p.ID == entity.ID)
                                        sType = "PlayerGroupDead";
                            }
                            catch { }
                        }

                        if ((sType == "Player") && (player.ID != entity.ID) && (!this.playersToolStripMenuItem.Checked)) continue;
                        if ((sType == "PlayerDead") && (!this.playersToolStripMenuItem.Checked)) continue;
                        if ((sType == "PlayerHostile") && (!this.playershostileToolStripMenuItem.Checked)) continue;
                        if ((sType == "PlayerGroup") && (!this.playersgroupToolStripMenuItem.Checked)) continue;
                        if ((sType == "PlayerGroupDead") && (!this.playersgroupToolStripMenuItem.Checked)) continue;

                        if ((sType == "NPC") && (!this.nPCsotherToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCDead") && (!this.nPCsdeadToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCLoot") && (!this.nPCsdeadToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCFriendly") && (!this.nPCsToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCAttackable") && (!this.nPCspeacefulToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCHostile") && (!this.nPCshostileToolStripMenuItem.Checked)) continue;
                        if ((sType == "NPCHostileLow") && (!this.nPCshostilelowToolStripMenuItem.Checked)) continue;

                        if ((sType == "Gatherable") && (!this.gatherableToolStripMenuItem.Checked)) continue;
                        if ((sType == "GatherableAir") && (!this.gatherableairToolStripMenuItem.Checked)) continue;
                        if ((sType == "Object") && (!this.objectsToolStripMenuItem.Checked)) continue;
                        if ((sType == "Vendor") && (!this.vendorsShopsToolStripMenuItem.Checked)) continue;
                        if ((sType == "Landmark") && (!this.landmarksToolStripMenuItem.Checked || this.bCompassMode)) continue;

                        try
                        {
                            if ((showDescriptionsToolStripMenuItem.Checked) && (!this.bCompassMode))
                            {
                                if (sType == "PlayerHostile")
                                    Utils.OutputString(ref offScreenDC, sDescOutput, fTextImportant, bTextWhite, bTextShadowFire, new Point(satX + 1, satY + 2));
                                else if ((entity.ID == player.TargetID) && (entity.ID != 0))
                                    Utils.OutputString(ref offScreenDC, sDescOutput, fTextImportant, bTextWhite, bTextShadowNuclear, new Point(satX + 1, satY + 2));
                                else
                                    Utils.OutputString(ref offScreenDC, sDescOutput /* + "/" + entity.ID + "/" + player.TargetID */, fTextNormal, brushes[sType], bTextShadow, new Point(satX + 1, satY + 2));
                            }
                            if (!sType.StartsWith("Player") || !this.showHealthToolStripMenuItem.Checked || this.bCompassMode || !this.classIconsForPlayersToolStripMenuItem.Checked)
                                offScreenDC.DrawIcon(icons[sType], satX - 5, satY - 4);

                            if (sType.StartsWith("Player") || (sType.StartsWith("NPC") && (sType != "NPCFriendly") && (sType != "NPC")))
                            {
                                if (this.showHealthToolStripMenuItem.Checked)
                                {
                                    if (this.bCompassMode)
                                    {
                                        offScreenDC.DrawRectangle(new Pen(Color.Black, 3), satX - 7, satY + 5, 25, 1);
                                        offScreenDC.DrawRectangle(new Pen(Color.FromArgb(0xA2, 0x48, 0x40), 1), satX - 7, satY + 5, entity.Health / 4, 1);
                                    }
                                    else
                                    {
                                        offScreenDC.DrawRectangle(new Pen(Color.Black, 5), satX + 6, satY + 18, 75, 1);
                                        offScreenDC.DrawRectangle(new Pen(Color.FromArgb(0xA2, 0x48, 0x40), 3), satX + 6, satY + 18, entity.Health * 3 / 4, 1);
  
                                    }
                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                if (showDescriptionsToolStripMenuItem.Checked)
                                    Utils.OutputString(ref offScreenDC, sDescOutput, fTextNormal, brushes["Unknown"], bTextShadow, new Point(satX + 1, satY + 2));
                                offScreenDC.DrawIcon(icons["Unknown"], satX - 5, satY - 4);
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            // Transfer the offscreen rendering to the actual PictureBox control
            Graphics g = picRadar.CreateGraphics();
            g.DrawImage(offScreenBmp, 0, 0);
            g.Dispose();
            offScreenBmp.Dispose();

            // If we don't do this, the radar will eat up ALL MEMORY!
            Utils.DoGarbageCollection();
        }

        /// <summary>
        /// Converts a game coordinant to its relative position in the PictureBox
        /// for drawing purposes.
        /// </summary>
        private void LocConverter(double TargetPosX, double TargetPosY, double OurPosX, double OurPosY, int NewCenterX, int NewCenterY, double OurRot, out int satX, out int satY)
        {
            satX = 0;
            satY = 0;

            double dWinWidth = this.picRadar.Width;
            double dWinHeight = this.picRadar.Height;

            double dZoomLevel_real = 133.3 * (100.0 / this.dZoomLevel);

            double dCoordMinX = OurPosX - dZoomLevel_real;
            double dCoordMinY = OurPosY - dZoomLevel_real;
            double dCoordMaxX = OurPosX + dZoomLevel_real;
            double dCoordMaxY = OurPosY + dZoomLevel_real;

            double dPercMinMaxX = (TargetPosX - dCoordMinX) / (dCoordMaxX - dCoordMinX);
            double dPercMinMaxY = (TargetPosY - dCoordMinY) / (dCoordMaxY - dCoordMinY);

            double dFinalCoordX = dWinWidth * dPercMinMaxX;
            double dFinalCoordY = dWinHeight * dPercMinMaxY;

            // Rotate based on camera angle
            using (Matrix m = new Matrix())
            {
                m.RotateAt((float)OurRot, new PointF((float)dWinWidth / 2, (float)dWinHeight / 2));
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(new RectangleF((float)dFinalCoordX, (float)dFinalCoordY, 3, 3));
                gp.Transform(m);
                PointF[] pts = gp.PathPoints;
                satX = (int)pts[0].X;
                satY = (int)pts[0].Y;
                gp.Dispose();
            }

            // Fix our parameters passed with 'out' keyword.
            int centerX = this.picRadar.Width / 2;
            if (satX > centerX) satX -= (satX - centerX) * 2;
            else if (satX < centerX) satX += (centerX - satX) * 2;
            return;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.FileVersionInfo fv = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

            MessageBox.Show(
                "Version " + fv.FileMajorPart + "." + Convert.ToInt32(fv.FileMinorPart).ToString("00") + ", build ID " + fv.FileBuildPart + "." + fv.FilePrivatePart + "\r\n" +
                "\r\n" +
                "Written by Pete\r\n" +
                "AionRadar@gmail.com\r\n" +
                "Copyright © " + ((DateTime.Now.Year <= 2009) ? "2009" : "2009-" + DateTime.Now.Year.ToString()) + " www.Aion-Radar.com\r\n" +
                "Built " + ((DateTime.Now.Year <= 2009) ? "2009" : "2009-" + DateTime.Now.Year.ToString()) + "\r\n" +
                "All Rights Reserved.",
                "About " + Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void compassModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bCompassMode = this.compassModeToolStripMenuItem.Checked;
            if (bCompassMode)
            {
                this.Width = 225;
                this.Height = 225;
            }
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            this.dZoomLevel = 100.0;
            this.toolStripMenuItem9.Checked = true;
            this.toolStripMenuItem13.Checked = false;
            this.toolStripMenuItem10.Checked = false;
            this.toolStripMenuItem11.Checked = false;
            this.toolStripMenuItem12.Checked = false;
        }
        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            this.dZoomLevel = 150.0;
            this.toolStripMenuItem9.Checked = false;
            this.toolStripMenuItem13.Checked = true;
            this.toolStripMenuItem10.Checked = false;
            this.toolStripMenuItem11.Checked = false;
            this.toolStripMenuItem12.Checked = false;
        }
        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            this.dZoomLevel = 200.0;
            this.toolStripMenuItem9.Checked = false;
            this.toolStripMenuItem13.Checked = false;
            this.toolStripMenuItem10.Checked = true;
            this.toolStripMenuItem11.Checked = false;
            this.toolStripMenuItem12.Checked = false;
        }
        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            this.dZoomLevel = 300.0;
            this.toolStripMenuItem9.Checked = false;
            this.toolStripMenuItem13.Checked = false;
            this.toolStripMenuItem10.Checked = false;
            this.toolStripMenuItem11.Checked = true;
            this.toolStripMenuItem12.Checked = false;
        }
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            this.dZoomLevel = 400.0;
            this.toolStripMenuItem9.Checked = false;
            this.toolStripMenuItem13.Checked = false;
            this.toolStripMenuItem10.Checked = false;
            this.toolStripMenuItem11.Checked = false;
            this.toolStripMenuItem12.Checked = true;
        }
    }
}
