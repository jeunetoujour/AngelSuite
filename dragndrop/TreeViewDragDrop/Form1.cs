using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace TreeViewDragDrop
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		// Node being dragged
		private TreeNode dragNode = null;
		
		// Temporary drop node for selection
		private TreeNode tempDropNode = null;

		// Timer for scrolling
		private Timer timer = new Timer();

		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ImageList imageListDrag;
		private System.Windows.Forms.ImageList imageListTreeView;
		private System.Windows.Forms.TreeView skilltree;


		public Form1()
		{
			InitializeComponent();

			this.skilltree.ExpandAll();

			timer.Interval = 200;
			timer.Tick += new EventHandler(timer_Tick);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node8");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node9");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Node10");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Node0");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Node11", new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            this.skilltree = new System.Windows.Forms.TreeView();
            this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // skilltree
            // 
            this.skilltree.AllowDrop = true;
            this.skilltree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skilltree.ImageIndex = 0;
            this.skilltree.ImageList = this.imageListTreeView;
            this.skilltree.Indent = 19;
            this.skilltree.Location = new System.Drawing.Point(0, 0);
            this.skilltree.Name = "skilltree";
            treeNode1.Name = "";
            treeNode1.Text = "Node7";
            treeNode2.Name = "";
            treeNode2.Text = "Node2";
            treeNode3.Name = "";
            treeNode3.Text = "Node5";
            treeNode4.Name = "";
            treeNode4.Text = "Node6";
            treeNode5.Name = "";
            treeNode5.Text = "Node1";
            treeNode6.Name = "";
            treeNode6.Text = "Node0";
            treeNode7.Name = "";
            treeNode7.Text = "Node8";
            treeNode8.Name = "";
            treeNode8.Text = "Node9";
            treeNode9.Name = "";
            treeNode9.Text = "Node10";
            treeNode10.Name = "";
            treeNode10.Text = "Node3";
            treeNode11.Name = "";
            treeNode11.Text = "Node4";
            treeNode12.Name = "";
            treeNode12.Text = "Node0";
            treeNode13.Name = "";
            treeNode13.Text = "Node1";
            treeNode14.Name = "";
            treeNode14.Text = "Node2";
            treeNode15.Name = "";
            treeNode15.Text = "Node3";
            treeNode16.Name = "";
            treeNode16.Text = "Node4";
            treeNode17.Name = "";
            treeNode17.Text = "Node5";
            treeNode18.Name = "";
            treeNode18.Text = "Node6";
            treeNode19.Name = "";
            treeNode19.Text = "Node7";
            treeNode20.Name = "";
            treeNode20.Text = "Node11";
            this.skilltree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode10,
            treeNode11,
            treeNode20});
            this.skilltree.SelectedImageIndex = 0;
            this.skilltree.Size = new System.Drawing.Size(534, 422);
            this.skilltree.TabIndex = 0;
            this.skilltree.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.treeView1_GiveFeedback);
            this.skilltree.DragLeave += new System.EventHandler(this.treeView1_DragLeave);
            this.skilltree.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.skilltree.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            this.skilltree.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.skilltree.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
            // 
            // imageListTreeView
            // 
            this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
            this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListTreeView.Images.SetKeyName(0, "");
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(534, 422);
            this.Controls.Add(this.skilltree);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Tree View Drag & Drop";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}


		private void treeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			// Get drag node and select it
			this.dragNode = (TreeNode)e.Item;
			this.skilltree.SelectedNode = this.dragNode;

			// Reset image list used for drag image
			this.imageListDrag.Images.Clear();
			this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.skilltree.Indent, this.dragNode.Bounds.Height);

			// Create new bitmap
			// This bitmap will contain the tree node image to be dragged
			Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.skilltree.Indent, this.dragNode.Bounds.Height);

			// Get graphics from bitmap
			Graphics gfx = Graphics.FromImage(bmp);

			// Draw node icon into the bitmap
			gfx.DrawImage(this.imageListTreeView.Images[0], 0, 0);

			// Draw node label into bitmap
			gfx.DrawString(this.dragNode.Text,
				this.skilltree.Font,
				new SolidBrush(this.skilltree.ForeColor),
				(float)this.skilltree.Indent, 1.0f);

			// Add bitmap to imagelist
			this.imageListDrag.Images.Add(bmp);

			// Get mouse position in client coordinates
			Point p = this.skilltree.PointToClient(Control.MousePosition);

			// Compute delta between mouse position and node bounds
			int dx = p.X + this.skilltree.Indent - this.dragNode.Bounds.Left;
			int dy = p.Y - this.dragNode.Bounds.Top;

			// Begin dragging image
			if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
			{
				// Begin dragging
				this.skilltree.DoDragDrop(bmp, DragDropEffects.Move);
				// End dragging image
				DragHelper.ImageList_EndDrag();
			}		
		
		}

		private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Compute drag position and move image
			Point formP = this.PointToClient(new Point(e.X, e.Y));
			DragHelper.ImageList_DragMove(formP.X - this.skilltree.Left, formP.Y - this.skilltree.Top);

			// Get actual drop node
			TreeNode dropNode = this.skilltree.GetNodeAt(this.skilltree.PointToClient(new Point(e.X, e.Y)));
			if(dropNode == null)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			
			e.Effect = DragDropEffects.Move;

			// if mouse is on a new node select it
			if(this.tempDropNode != dropNode)
			{
				DragHelper.ImageList_DragShowNolock(false);
				this.skilltree.SelectedNode = dropNode;
				DragHelper.ImageList_DragShowNolock(true);
				tempDropNode = dropNode;
			}
			
			// Avoid that drop node is child of drag node 
			TreeNode tmpNode = dropNode;
			while(tmpNode.Parent != null)
			{
				if(tmpNode.Parent == this.dragNode) e.Effect = DragDropEffects.None;
				tmpNode = tmpNode.Parent;
			}
		}

		private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Unlock updates
			DragHelper.ImageList_DragLeave(this.skilltree.Handle);

			// Get drop node
			TreeNode dropNode = this.skilltree.GetNodeAt(this.skilltree.PointToClient(new Point(e.X, e.Y)));

			// If drop node isn't equal to drag node, add drag node as child of drop node
			if(this.dragNode != dropNode)
			{
				// Remove drag node from parent
				if(this.dragNode.Parent == null)
				{
					this.skilltree.Nodes.Remove(this.dragNode);
				}
				else
				{
					this.dragNode.Parent.Nodes.Remove(this.dragNode);
				}

				// Add drag node to drop node
				//dropNode.Nodes.Add(this.dragNode);
                dropNode.Parent.Nodes.Add(this.dragNode);
				dropNode.ExpandAll();

				// Set drag node to null
				this.dragNode = null;

				// Disable scroll timer
				this.timer.Enabled = false;
			}
		}

		private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			DragHelper.ImageList_DragEnter(this.skilltree.Handle, e.X - this.skilltree.Left,
				e.Y - this.skilltree.Top);

			// Enable timer for scrolling dragged item
			this.timer.Enabled = true;
		}

		private void treeView1_DragLeave(object sender, System.EventArgs e)
		{
			DragHelper.ImageList_DragLeave(this.skilltree.Handle);

			// Disable timer for scrolling dragged item
			this.timer.Enabled = false;
		}

		private void treeView1_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			if(e.Effect == DragDropEffects.Move) 
			{
				// Show pointer cursor while dragging
				e.UseDefaultCursors = false;
				this.skilltree.Cursor = Cursors.Default;
			}
			else e.UseDefaultCursors = true;
			
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			// get node at mouse position
			Point pt = PointToClient(Control.MousePosition);
			TreeNode node = this.skilltree.GetNodeAt(pt);

			if(node == null) return;

			// if mouse is near to the top, scroll up
			if(pt.Y < 30)
			{
				// set actual node to the upper one
				if (node.PrevVisibleNode!= null) 
				{
					node = node.PrevVisibleNode;
				
					// hide drag image
					DragHelper.ImageList_DragShowNolock(false);
					// scroll and refresh
					node.EnsureVisible();
					this.skilltree.Refresh();
					// show drag image
					DragHelper.ImageList_DragShowNolock(true);
					
				}
			}
			// if mouse is near to the bottom, scroll down
			else if(pt.Y > this.skilltree.Size.Height - 30)
			{
				if (node.NextVisibleNode!= null) 
				{
					node = node.NextVisibleNode;
				
					DragHelper.ImageList_DragShowNolock(false);
					node.EnsureVisible();
					this.skilltree.Refresh();
					DragHelper.ImageList_DragShowNolock(true);
				}
			} 
		}

	}
}
