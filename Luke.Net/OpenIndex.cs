using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Summary description for OpenIndex.
	/// </summary>
	public class OpenIndex : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.CheckBox checkRO;
		private System.Windows.Forms.CheckBox checkUnlock;
		private System.Windows.Forms.Label separatorOverview;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ComboBox textPath;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Preferences p;
		public OpenIndex(Preferences p)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.p = p;

			foreach (string path in p.MruList)
			{
				textPath.Items.Add(path);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenIndex));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.checkRO = new System.Windows.Forms.CheckBox();
            this.checkUnlock = new System.Windows.Forms.CheckBox();
            this.separatorOverview = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textPath = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Path:";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(312, 8);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "&Browse...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // checkRO
            // 
            this.checkRO.Location = new System.Drawing.Point(8, 40);
            this.checkRO.Name = "checkRO";
            this.checkRO.Size = new System.Drawing.Size(192, 24);
            this.checkRO.TabIndex = 3;
            this.checkRO.Text = "Open in &Read-Only mode";
            // 
            // checkUnlock
            // 
            this.checkUnlock.Location = new System.Drawing.Point(8, 64);
            this.checkUnlock.Name = "checkUnlock";
            this.checkUnlock.Size = new System.Drawing.Size(192, 24);
            this.checkUnlock.TabIndex = 4;
            this.checkUnlock.Text = "&Force unlock, if locked";
            // 
            // separatorOverview
            // 
            this.separatorOverview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorOverview.Location = new System.Drawing.Point(8, 88);
            this.separatorOverview.Name = "separatorOverview";
            this.separatorOverview.Size = new System.Drawing.Size(376, 2);
            this.separatorOverview.TabIndex = 11;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(232, 96);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(312, 96);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            // 
            // textPath
            // 
            this.textPath.Location = new System.Drawing.Point(40, 9);
            this.textPath.Name = "textPath";
            this.textPath.Size = new System.Drawing.Size(264, 21);
            this.textPath.TabIndex = 1;
            // 
            // OpenIndex
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(392, 126);
            this.Controls.Add(this.textPath);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.separatorOverview);
            this.Controls.Add(this.checkUnlock);
            this.Controls.Add(this.checkRO);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenIndex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Path to Index Directory";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void buttonBrowse_Click(object sender, System.EventArgs e)
		{
			FolderBrowser folderBrowser = new FolderBrowser();
			
			if (folderBrowser.ShowDialog(this) == DialogResult.OK)
				textPath.Text = folderBrowser.DirectoryPath;
		}
		
		public bool ReadOnlyIndex
		{
			get
			{ return checkRO.Checked; }
			set
			{ checkRO.Checked = value; }
		}
		
		public bool UnlockIndex
		{
			get
			{ return checkUnlock.Checked; }
			set
			{ checkUnlock.Checked = value; }
		}
		
		public String Path
		{
			get
			{ return textPath.Text.Trim(); }
		}
	}
}
