using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Summary description for Progress.
	/// </summary>
	public class Progress : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar progressExplain;
		private System.Windows.Forms.Label lblMsg;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Progress()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		public string Message
		{
			set
			{lblMsg.Text = value;}
		}

		public int Value
		{
			set
			{progressExplain.Value = value;}
			get
			{return progressExplain.Value;}
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
			this.progressExplain = new System.Windows.Forms.ProgressBar();
			this.lblMsg = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressExplain
			// 
			this.progressExplain.Location = new System.Drawing.Point(8, 8);
			this.progressExplain.Name = "progressExplain";
			this.progressExplain.Size = new System.Drawing.Size(152, 23);
			this.progressExplain.TabIndex = 0;
			// 
			// lblMsg
			// 
			this.lblMsg.Location = new System.Drawing.Point(8, 40);
			this.lblMsg.Name = "lblMsg";
			this.lblMsg.Size = new System.Drawing.Size(152, 16);
			this.lblMsg.TabIndex = 1;
			// 
			// Progress
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(170, 66);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblMsg,
																		  this.progressExplain});
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Progress";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Action progress";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
