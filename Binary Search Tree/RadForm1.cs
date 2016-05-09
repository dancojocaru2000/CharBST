using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using TreeGenerator;

namespace Binary_Search_Tree {
	public partial class RadForm1 : Telerik.WinControls.UI.RadForm {
		public RadForm1() {
			ThemeResolutionService.ApplicationThemeName = "VisualStudio2012Dark";
			InitializeComponent();
		}

		TreeGenerator.TreeData.TreeDataTableDataTable dtTree;

		public void GraphicTreeConstructorWorker(BinaryTreeNode worker, BigInteger position, int state = 0) {
			if (state == 0) dtTree.AddTreeDataTableRow(position.ToString(), "", string.Format("Root: {0}", worker.Value), "");
			else if (state == 1) dtTree.AddTreeDataTableRow(position.ToString(), (position / 2).ToString(), string.Format("Right node: {0}", worker.Value), "");
			else dtTree.AddTreeDataTableRow(position.ToString(), (position / 2).ToString(), string.Format("Left node: {0}", worker.Value), "");
			if (worker.left != null && worker.right != null) {
				GraphicTreeConstructorWorker(worker.left, position * 2, -1);
				GraphicTreeConstructorWorker(worker.right, position * 2 + 1, 1);
			}
			else if (worker.left == null && worker.right == null) ;
			else {
				if (worker.left == null) dtTree.AddTreeDataTableRow((position * 2).ToString(), position.ToString(), "", "");
				else GraphicTreeConstructorWorker(worker.left, position * 2, -1);
				if (worker.right == null) dtTree.AddTreeDataTableRow((position * 2 + 1).ToString(), position.ToString(), "", "");
				else GraphicTreeConstructorWorker(worker.right, position * 2 + 1, 1);
			}
		}

		public void GraphicTreeConstructor() {
			if (tree.root != null) {
				pictureBox1.Visible = true;
				dtTree = new TreeData.TreeDataTableDataTable();
				GraphicTreeConstructorWorker(tree.root, new BigInteger(1));
				var myTree = new TreeBuilder(dtTree);
				myTree.BGColor = myTree.BoxFillColor = Color.Black;
				myTree.FontColor = Color.Yellow;
				myTree.LineColor = Color.LightBlue;
				//myTree.HorizontalSpace = myTree.VerticalSpace = 10;
				if (!FullSize) pictureBox1.Image = Image.FromStream(myTree.GenerateTree(pictureBox1.Width, pictureBox1.Height, "1", System.Drawing.Imaging.ImageFormat.Bmp));
				else {
					pictureBox1.Image = Image.FromStream(myTree.GenerateTree("1", System.Drawing.Imaging.ImageFormat.Bmp));
					pictureBox1.Width = pictureBox1.Image.Width;
					pictureBox1.Height = pictureBox1.Image.Height;
				}
			}
			else {
				pictureBox1.Visible = false;
			}
		}

		BinarySearchTree tree = new BinarySearchTree();

		/*public static RadTreeNode TreeUpdate(ref BinaryTreeNode position, int state, bool ExpandRoot, bool ExpandLeftNodes, bool ExpandRightNodes) {
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
		}*/

		public void UIUpdate(bool ExpandRoot = true, bool ExpandLeftNodes = true, bool ExpandRightNodes = true) {
			elementsLabel.Text = string.Format("Elements: {0}", tree.Elements.ToString());
			depthLabel.Text = string.Format("Depth: {0}", tree.Depth.ToString());
			iplLabel.Text = string.Format("Internal Path Lenght: {0}", tree.InternalPathLenght.ToString());
			eplLabel.Text = string.Format("External Path Lenght: {0}", tree.ExternalPathLenght.ToString());
			preorderLabel.Text = "Preorder tree: " + tree.PreorderValues;
			inorderLabel.Text = "Inorder tree: " + tree.InorderValues;
			postorderLabel.Text = "Postorder tree: " + tree.PostorderValues;
			GraphicTreeConstructor();
			//radTreeView1.Nodes.Clear();
			//if (tree.root != null) radTreeView1.Nodes.Add(TreeUpdate(ref tree.root, 0, ExpandRoot, ExpandLeftNodes, ExpandRightNodes));
		}

		private void add() {
			while (radTextBox1.Text != string.Empty) {
				if ((radTextBox1.Text[0] >= 'A' && radTextBox1.Text[0] <= 'Z') || (radTextBox1.Text[0] >= 'a' && radTextBox1.Text[0] <= 'z') || (radTextBox1.Text[0] >= '0' && radTextBox1.Text[0] <= '9')) tree.AddValue(radTextBox1.Text[0]);
				radTextBox1.Text = radTextBox1.Text.Remove(0, 1);
			}
			UIUpdate(radCheckBox1.Checked, radCheckBox2.Checked, radCheckBox3.Checked);
		}

		private void radButton1_Click(object sender, EventArgs e) {
			add();
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

		private void pictureBox1_SizeChanged(object sender, EventArgs e) {
			GraphicTreeConstructor();
		}

		private void radButtonElement1_Click(object sender, EventArgs e) {
			MissionControl.Visible = !MissionControl.Visible;
			if (MissionControl.Visible) {
				radButtonElement1.Text = "Hide Mission Control";
			} else {
				radButtonElement1.Text = "Show Mission Control";
			}
		}

		public bool FullSize = false;

		private void radRadioButton2_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			if (radRadioButton2.IsChecked == true) {
				radRadioButton1.IsChecked = false;
				FullSize = true;
				pictureBox1.Dock = DockStyle.None;
				pictureBox1.Left = 0;
				pictureBox1.Top = 0;
				UIUpdate();
			}
		}

		private void radRadioButton1_ToggleStateChanged(object sender, StateChangedEventArgs args) {
			if (radRadioButton1.IsChecked == true) {
				radRadioButton2.IsChecked = false;
				FullSize = !true;
				pictureBox1.Dock = DockStyle.Fill;
				UIUpdate();
			}
		}

		private void radButton3_Click(object sender, EventArgs e) {
			dtTree = new TreeData.TreeDataTableDataTable();
			GraphicTreeConstructorWorker(tree.root, new BigInteger(1));
			var myTree = new TreeBuilder(dtTree);
			myTree.BGColor = myTree.BoxFillColor = Color.Black;
			myTree.FontColor = Color.Yellow;
			myTree.LineColor = Color.LightBlue;
			var image = Image.FromStream(myTree.GenerateTree("1", System.Drawing.Imaging.ImageFormat.Bmp));
			var x = saveFileDialog1.ShowDialog();
			if (x == DialogResult.OK) {
				if (saveFileDialog1.FileName.EndsWith(".bmp"))
					image.Save(saveFileDialog1.FileName);
				else if (saveFileDialog1.FileName.EndsWith(".png")) {
					var stream = new System.IO.MemoryStream();
					image.Save(stream, ImageFormat.Png);
					stream.Position = 0;
					var fileStream = System.IO.File.Create(saveFileDialog1.FileName);
					stream.Seek(0, System.IO.SeekOrigin.Begin);
					stream.CopyTo(fileStream);
				}
				else if (saveFileDialog1.FileName.EndsWith(".jpg")) {
					var stream = new System.IO.MemoryStream();
					image.Save(stream, ImageFormat.Jpeg);
					stream.Position = 0;
					var fileStream = System.IO.File.Create(saveFileDialog1.FileName);
					stream.Seek(0, System.IO.SeekOrigin.Begin);
					stream.CopyTo(fileStream);
				}
			}
		}

		private void radTextBox1_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				add();
			}
		}
	}
}
