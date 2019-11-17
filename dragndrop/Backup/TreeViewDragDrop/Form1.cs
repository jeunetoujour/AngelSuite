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
		private System.Windows.Forms.TreeView treeView1;


		public Form1()
		{
			InitializeComponent();

			this.treeView1.ExpandAll();

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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.imageListTreeView = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// imageListDrag
			// 
			this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// treeView1
			// 
			this.treeView1.AllowDrop = true;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.ImageList = this.imageListTreeView;
			this.treeView1.Indent = 19;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																				  new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
																																									 new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
																																																														new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
																																																																																		   new System.Windows.Forms.TreeNode("Node7")}),
																																																														new System.Windows.Forms.TreeNode("Node5"),
																																																														new System.Windows.Forms.TreeNode("Node6")})}),
																				  new System.Windows.Forms.TreeNode("Node3", new System.Windows.Forms.TreeNode[] {
																																									 new System.Windows.Forms.TreeNode("Node8"),
																																									 new System.Windows.Forms.TreeNode("Node9"),
																																									 new System.Windows.Forms.TreeNode("Node10")}),
																				  new System.Windows.Forms.TreeNode("Node4"),
																				  new System.Windows.Forms.TreeNode("Node11", new System.Windows.Forms.TreeNode[] {
																																									  new System.Windows.Forms.TreeNode("Node0"),
																																									  new System.Windows.Forms.TreeNode("Node1"),
																																									  new System.Windows.Forms.TreeNode("Node2"),
																																									  new System.Windows.Forms.TreeNode("Node3"),
																																									  new System.Windows.Forms.TreeNode("Node4"),
																																									  new System.Windows.Forms.TreeNode("Node5"),
																																									  new System.Windows.Forms.TreeNode("Node6"),
																																									  new System.Windows.Forms.TreeNode("Node7")})});
			this.treeView1.Size = new System.Drawing.Size(272, 237);
			this.treeView1.TabIndex = 0;
			this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView1_DragOver);
			this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
			this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
			this.treeView1.DragLeave += new System.EventHandler(this.treeView1_DragLeave);
			this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
			this.treeView1.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.treeView1_GiveFeedback);
			// 
			// imageListTreeView
			// 
			this.imageListTreeView.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageListTreeView.ImageSize = new System.Drawing.Size(16, 16);
			this.imageListTreeView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListTreeView.ImageStream")));
			this.imageListTreeView.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(272, 237);
			this.Controls.Add(this.treeView1);
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
			this.treeView1.SelectedNode = this.dragNode;

			// Reset image list used for drag image
			this.imageListDrag.Images.Clear();
			this.imageListDrag.ImageSize = new Size(this.dragNode.Bounds.Size.Width + this.treeView1.Indent, this.dragNode.Bounds.Height);

			// Create new bitmap
			// This bitmap will contain the tree node image to be dragged
			Bitmap bmp = new Bitmap(this.dragNode.Bounds.Width + this.treeView1.Indent, this.dragNode.Bounds.Height);

			// Get graphics from bitmap
			Graphics gfx = Graphics.FromImage(bmp);

			// Draw node icon into the bitmap
			gfx.DrawImage(this.imageListTreeView.Images[0], 0, 0);

			// Draw node label into bitmap
			gfx.DrawString(this.dragNode.Text,
				this.treeView1.Font,
				new SolidBrush(this.treeView1.ForeColor),
				(float)this.treeView1.Indent, 1.0f);

			// Add bitmap to imagelist
			this.imageListDrag.Images.Add(bmp);

			// Get mouse position in client coordinates
			Point p = this.treeView1.PointToClient(Control.MousePosition);

			// Compute delta between mouse position and node bounds
			int dx = p.X + this.treeView1.Indent - this.dragNode.Bounds.Left;
			int dy = p.Y - this.dragNode.Bounds.Top;

			// Begin dragging image
			if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, dx, dy))
			{
				// Begin dragging
				this.treeView1.DoDragDrop(bmp, DragDropEffects.Move);
				// End dragging image
				DragHelper.ImageList_EndDrag();
			}		
		
		}

		private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			// Compute drag position and move image
			Point formP = this.PointToClient(new Point(e.X, e.Y));
			DragHelper.ImageList_DragMove(formP.X - this.treeView1.Left, formP.Y - this.treeView1.Top);

			// Get actual drop node
			TreeNode dropNode = this.treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));
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
				this.treeView1.SelectedNode = dropNode;
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
			DragHelper.ImageList_DragLeave(this.treeView1.Handle);

			// Get drop node
			TreeNode dropNode = this.treeView1.GetNodeAt(this.treeView1.PointToClient(new Point(e.X, e.Y)));

			// If drop node isn't equal to drag node, add drag node as child of drop node
			if(this.dragNode != dropNode)
			{
				// Remove drag node from parent
				if(this.dragNode.Parent == null)
				{
					this.treeView1.Nodes.Remove(this.dragNode);
				}
				else
				{
					this.dragNode.Parent.Nodes.Remove(this.dragNode);
				}

				// Add drag node to drop node
				dropNode.Nodes.Add(this.dragNode);
				dropNode.ExpandAll();

				// Set drag node to null
				this.dragNode = null;

				// Disable scroll timer
				this.timer.Enabled = false;
			}
		}

		private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			DragHelper.ImageList_DragEnter(this.treeView1.Handle, e.X - this.treeView1.Left,
				e.Y - this.treeView1.Top);

			// Enable timer for scrolling dragged item
			this.timer.Enabled = true;
		}

		private void treeView1_DragLeave(object sender, System.EventArgs e)
		{
			DragHelper.ImageList_DragLeave(this.treeView1.Handle);

			// Disable timer for scrolling dragged item
			this.timer.Enabled = false;
		}

		private void treeView1_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
		{
			if(e.Effect == DragDropEffects.Move) 
			{
				// Show pointer cursor while dragging
				e.UseDefaultCursors = false;
				this.treeView1.Cursor = Cursors.Default;
			}
			else e.UseDefaultCursors = true;
			
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			// get node at mouse position
			Point pt = PointToClient(Control.MousePosition);
			TreeNode node = this.treeView1.GetNodeAt(pt);

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
					this.treeView1.Refresh();
					// show drag image
					DragHelper.ImageList_DragShowNolock(true);
					
				}
			}
			// if mouse is near to the bottom, scroll down
			else if(pt.Y > this.treeView1.Size.Height - 30)
			{
				if (node.NextVisibleNode!= null) 
				{
					node = node.NextVisibleNode;
				
					DragHelper.ImageList_DragShowNolock(false);
					node.EnsureVisible();
					this.treeView1.Refresh();
					DragHelper.ImageList_DragShowNolock(true);
				}
			} 
		}

	}
}
