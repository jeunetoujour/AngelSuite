using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using AionMemory;
using MemoryLib;
using System.Runtime.InteropServices;
using QuickGraph;
using QuickGraph.Graphviz;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetTickCount();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #region Output

        public void Clear()
        {
            outputSb = new StringBuilder();
            outputTextBox.Text = "";
        }

        public void Print(object thing)
        {
            outputSb.AppendLine(thing.ToString());
            outputTextBox.Text = outputSb.ToString();
        }

        StringBuilder outputSb = new StringBuilder();

        #endregion


        #region Traversal

        List<AbilityTreeNode> nodes = null;
        HashSet<int> seenNodes = null;

        void RecurseTree(AbilityTreeNode node)
        {
            if (!seenNodes.Add(node.Ptr))
                return;

            if (node.CheckValue3 == true) // shit node value?
                return;

            nodes.Add(node);

            RecurseTree(node.Left);
            RecurseTree(node.Parent); // pointless.
            RecurseTree(node.Right);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Process.Open();

            Clear();

            int headPtr = Memory.ReadInt(Process.handle, Process.Modules.Game + 0x8e3930);
            headPtr = Memory.ReadInt(Process.handle, headPtr + 0x1e4);
            Print(headPtr);

            AbilityTreeNode head = new AbilityTreeNode(headPtr);
            nodes = new List<AbilityTreeNode>();
            seenNodes = new HashSet<int>();
            RecurseTree(head);

            Print(DateTime.Now);
            foreach (var node in nodes)
            {
                Print(string.Format("{0}: (nodePtr: {1}, abilityPtr: {2}, abilityCooldownMax: {3}, abilityCooldownTS: {4})", node.AbilityID, node.Ptr.ToString("X"), node.AbilityPtr.ToString("X"), node.AbilityCooldownMax, node.AbilityCooldownTS));
            }

            List<TaggedEdge<int, string>> nodeEdges = new List<TaggedEdge<int, string>>();

            foreach (var node in nodes)
            {
                nodeEdges.Add(new TaggedEdge<int, string>(node.AbilityID, node.Left.AbilityID, "Left"));
                nodeEdges.Add(new TaggedEdge<int, string>(node.AbilityID, node.Parent.AbilityID, "Parent"));
                nodeEdges.Add(new TaggedEdge<int, string>(node.AbilityID, node.Right.AbilityID, "Right"));
            }

            var graph = nodeEdges.ToAdjacencyGraph<int, TaggedEdge<int, string>>();
            var graphviz = new GraphvizAlgorithm<int, TaggedEdge<int, string>>(graph);
            graphviz.FormatVertex += new FormatVertexEventHandler<int>(graphviz_FormatVertex);
            graphviz.FormatEdge += new FormatEdgeAction<int, TaggedEdge<int, string>>(graphviz_FormatEdge);
            Print(graphviz.Generate(new FileDotEngine(), "graph"));
        }

        void graphviz_FormatEdge(object sender, FormatEdgeEventArgs<int, TaggedEdge<int, string>> e)
        {
            e.EdgeFormatter.Label.Value = e.Edge.Tag;
        }

        void graphviz_FormatVertex(object sender, FormatVertexEventArgs<int> e)
        {
            e.VertexFormatter.Label = e.Vertex.ToString();
        }

        #endregion
    }
}
