using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Lucene.Net.Index;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Summary description for TermVector.
	/// </summary>
	public class TermVector : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Label lblField;
		private System.Windows.Forms.ColumnHeader colFreq;
		private System.Windows.Forms.ColumnHeader colTerm;
		private System.Windows.Forms.ListView listViewTVF;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TermVector(string fieldName, ITermFreqVector tfv)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			lblField.Text = fieldName;
			IntPair[] tvs = new IntPair[tfv.Size];
			String[] terms = tfv.GetTerms();
			int[] freqs = tfv.GetTermFrequencies();
			for (int i = 0; i < terms.Length; i++) 
			{
				IntPair ip = new IntPair(freqs[i], terms[i]);
				tvs[i] = ip;
			}
			Array.Sort(tvs, new IntPair.PairComparator(false, true));

			listViewTVF.BeginUpdate();

			for (int i = 0; i < tvs.Length; i++) 
			{
				ListViewItem item = new ListViewItem(
					new string[]{tvs[i].cnt.ToString(), tvs[i].text});
				listViewTVF.Items.Add(item);
			}

			listViewTVF.EndUpdate();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TermVector));
            this.btnOK = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblField = new System.Windows.Forms.Label();
            this.listViewTVF = new System.Windows.Forms.ListView();
            this.colFreq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTerm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(288, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            // 
            // lblInfo
            // 
            this.lblInfo.Image = ((System.Drawing.Image)(resources.GetObject("lblInfo.Image")));
            this.lblInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInfo.Location = new System.Drawing.Point(8, 8);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(136, 23);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Term vector for the field ";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblField
            // 
            this.lblField.Location = new System.Drawing.Point(144, 8);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(208, 23);
            this.lblField.TabIndex = 2;
            this.lblField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listViewTVF
            // 
            this.listViewTVF.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFreq,
            this.colTerm});
            this.listViewTVF.FullRowSelect = true;
            this.listViewTVF.GridLines = true;
            this.listViewTVF.Location = new System.Drawing.Point(8, 32);
            this.listViewTVF.Name = "listViewTVF";
            this.listViewTVF.Size = new System.Drawing.Size(352, 200);
            this.listViewTVF.TabIndex = 3;
            this.listViewTVF.UseCompatibleStateImageBehavior = false;
            this.listViewTVF.View = System.Windows.Forms.View.Details;
            // 
            // colFreq
            // 
            this.colFreq.Text = "Freq";
            this.colFreq.Width = 50;
            // 
            // colTerm
            // 
            this.colTerm.Text = "Term";
            this.colTerm.Width = 298;
            // 
            // TermVector
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(370, 271);
            this.Controls.Add(this.listViewTVF);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TermVector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Term Vector";
            this.ResumeLayout(false);

		}
		#endregion
	}
}
