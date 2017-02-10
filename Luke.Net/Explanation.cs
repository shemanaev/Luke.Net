using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Lucene.Net.Search;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Summary description for Explanation.
	/// </summary>
	public class ExplanationDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.TreeView treeExplain;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExplanationDialog(Explanation e)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			treeExplain.BeginUpdate();
			AddNode(null, e);
			treeExplain.ExpandAll();
			treeExplain.EndUpdate();
		}

		private void AddNode(TreeNode tn, Explanation e)
		{
			TreeNode node = new TreeNode(e.Value.ToString("G4") + "  " + e.Description);

			if (null == tn)
			{
				treeExplain.Nodes.Add(node);
			}
			else
			{
				tn.Nodes.Add(node);
			}

			Explanation[] kids = e.GetDetails();
			if (kids != null && kids.Length > 0) 
			{
				for (int i = 0; i < kids.Length; i++) 
				{
					AddNode(node, kids[i]);
				}
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExplanationDialog));
			this.lblInfo = new System.Windows.Forms.Label();
			this.treeExplain = new System.Windows.Forms.TreeView();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblInfo
			// 
			this.lblInfo.Image = ((System.Drawing.Bitmap)(resources.GetObject("lblInfo.Image")));
			this.lblInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblInfo.Location = new System.Drawing.Point(8, 8);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(176, 23);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "Explanation of the document hit:";
			this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// treeExplain
			// 
			this.treeExplain.ImageIndex = -1;
			this.treeExplain.Location = new System.Drawing.Point(8, 32);
			this.treeExplain.Name = "treeExplain";
			this.treeExplain.SelectedImageIndex = -1;
			this.treeExplain.Size = new System.Drawing.Size(320, 168);
			this.treeExplain.TabIndex = 1;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(253, 208);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			// 
			// ExplanationDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(338, 239);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnOK,
																		  this.treeExplain,
																		  this.lblInfo});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExplanationDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Explanation";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
