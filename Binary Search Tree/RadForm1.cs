using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Binary_Search_Tree {
	public partial class RadForm1 : Telerik.WinControls.UI.RadForm {
		public RadForm1() {
			ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
			InitializeComponent();
		}

		BinarySearchTree tree = new BinarySearchTree();

		public static RadTreeNode TreeUpdate(ref BinaryTreeNode position, int state, bool ExpandRoot, bool ExpandLeftNodes, bool ExpandRightNodes) {
			RadTreeNode node;
			if (state == 0) {
				node = new RadTreeNode(string.Format("Root: {0}", position.Value));
			} else if (state == 1) {
				node = new RadTreeNode(string.Format("Right node: {0}", position.Value));
			} else {
				node = new RadTreeNode(string.Format("Left node: {0}", position.Value));
			}
			if (position.left != null) node.Nodes.Add(TreeUpdate(ref position.left, -1, ExpandRoot, ExpandLeftNodes, ExpandRightNodes));
			if (position.right != null) node.Nodes.Add(TreeUpdate(ref position.right, 1, ExpandRoot, ExpandLeftNodes, ExpandRightNodes));
			if (state == 0) if (ExpandRoot) node.Expand();
			if (state == 1) if (ExpandRightNodes) node.Expand();
			if (state == -1) if (ExpandLeftNodes) node.Expand();
			return node;
		}

		public void UIUpdate(bool ExpandRoot = true, bool ExpandLeftNodes = true, bool ExpandRightNodes = true) {
			elementsLabel.Text = string.Format("Elements: {0}", tree.Elements.ToString());
			depthLabel.Text = string.Format("Depth: {0}", tree.Depth.ToString());
			iplLabel.Text = string.Format("Internal Path Lenght: {0}", tree.InternalPathLenght.ToString());
			eplLabel.Text = string.Format("External Path Lenght: {0}", tree.ExternalPathLenght.ToString());
			preorderLabel.Text = "Preorder tree: " + tree.PreorderValues;
			inorderLabel.Text = "Inorder tree: " + tree.InorderValues;
			postorderLabel.Text = "Postorder tree: " + tree.PostorderValues;
			radTreeView1.Nodes.Clear();
			if (tree.root != null) radTreeView1.Nodes.Add(TreeUpdate(ref tree.root, 0, ExpandRoot, ExpandLeftNodes, ExpandRightNodes));
		}

		private void radButton1_Click(object sender, EventArgs e) {
			while (radTextBox1.Text != string.Empty) {
				tree.AddValue(radTextBox1.Text[0]);
				radTextBox1.Text = radTextBox1.Text.Remove(0, 1);
				UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
			}
		}

		private void radButton2_Click(object sender, EventArgs e) {
			var x = RadMessageBox.Show("Do you want to reset the content of the tree?", "Tree reset", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation);
			if (x == DialogResult.Yes) {
				tree.Clear('Y');
				UIUpdate();
			}
		}

		private void radCheckBox4_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			if (radCheckBox4.Checked == true) {
				radCheckBox1.Checked = radCheckBox2.Checked = radCheckBox3.Checked = true;
				radCheckBox1.Enabled = radCheckBox2.Enabled = radCheckBox3.Enabled = false;
			} else {
				radCheckBox1.Checked = radCheckBox2.Checked = radCheckBox3.Checked = false;
				radCheckBox1.Enabled = true;
			}
			UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
		}

		private void radCheckBox1_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			if (radCheckBox1.Enabled == true) {
				radCheckBox2.Enabled = radCheckBox3.Enabled = true;
			} else {
				radCheckBox2.Enabled = radCheckBox3.Enabled = false;
				radCheckBox2.Checked = radCheckBox3.Checked = false;
			}
			UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
		}

		private void radCheckBox3_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
			if (radCheckBox2.Checked == true && radCheckBox2.Checked == radCheckBox3.Checked) radCheckBox4.Checked = true;
		}

		private void radCheckBox2_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
			if (radCheckBox2.Checked == true && radCheckBox2.Checked == radCheckBox3.Checked) radCheckBox4.Checked = true;
		}
	}
}
