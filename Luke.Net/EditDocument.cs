using System;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;

using Lucene.Net.Documents;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Summary description for EditDocument.
	/// </summary>
	public class EditDocument : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupDocNum;
		private System.Windows.Forms.Label lblAnalyzer;
		private System.Windows.Forms.ComboBox cmbAnalyzers;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.TextBox txtContent;
		private System.Windows.Forms.CheckBox chTVF;
		private System.Windows.Forms.CheckBox chTokenized;
		private System.Windows.Forms.CheckBox chIndexed;
		private System.Windows.Forms.CheckBox chStored;
		private System.Windows.Forms.Button btnDeleteField;
		private System.Windows.Forms.TextBox txtBoost;
		private System.Windows.Forms.Label lblBoost;
		private System.Windows.Forms.ListBox lstFields;
		private System.Windows.Forms.Label lblNote;
		private System.Windows.Forms.Button btnEditAddField;
		private System.Windows.Forms.TextBox txtNewName;
		private System.Windows.Forms.Panel panelEdit;
		private System.Windows.Forms.Label separator;
		private System.Windows.Forms.Button btnReplace;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblNewFieldName;
		private System.Windows.Forms.Label lblType;
		private System.ComponentModel.IContainer components;

		private int docNum;
		private Document document;
		private Luke luke;
		private ArrayList fields;
		private ArrayList fieldsReconstructed;
		private int lastIndex;

		private ResourceManager resources = new ResourceManager
			(
			typeof(Luke).Namespace + ".Messages",
			Assembly.GetAssembly(typeof(Luke))
			);	

		public EditDocument(Luke parent, int docNum, Document document, Hashtable doc)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.docNum = docNum;
			this.document = document;
			this.luke = parent;

			luke.PopulateAnalyzers(cmbAnalyzers);
			cmbAnalyzers.SelectedIndex = 0;

			groupDocNum.Text = groupDocNum.Text + docNum.ToString();

			ReconstructFields(doc);

			foreach(Field field in fields)
			{
				lstFields.Items.Add(field.Name);
			}

			if (lstFields.Items.Count > 0)
			{
				lstFields.SelectedIndex = 0;
				PopulateFieldContent(0);
			}
		}

		private void ReconstructFields(Hashtable doc)
		{
			fields = new ArrayList(doc.Count+5);
			fieldsReconstructed = new ArrayList(doc.Count+5);
			int i = 0;
			foreach (string key in doc.Keys) 
			{
				Object t = doc[key];

				// create/reconstruct fields
				if (typeof(Field).IsInstanceOfType(t)) 
				{
					fields.Add(t);
					fieldsReconstructed.Add(false);
				} 
				else 
				{
					fieldsReconstructed.Add(true);

					GrowableStringArray terms = (GrowableStringArray)doc[key];
					StringBuilder sb = new StringBuilder();
					if (terms != null)
					{
						String sNull = "null";
						int k = 0, m = 0;
						for (int j = 0; j < terms.Size(); j++) 
						{
							if (terms.Get(j) == null) k++;
							else 
							{
								if (sb.Length > 0) sb.Append(' ');
								if (k > 0) 
								{
									sb.Append(sNull + "_" + k + " ");
									k = 0;
									m++;
								}
								sb.Append(terms.Get(j));
								m++;
								if (m % 10 == 0) sb.Append('\n');
							}
						}
					}

					Field newField =  new Field(key, sb.ToString(),Field.Store.NO,Field.Index.ANALYZED);
					newField.Boost = document.Boost;
					fields.Add(newField);
				}
				i++;
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(EditDocument));
			this.groupDocNum = new System.Windows.Forms.GroupBox();
			this.btnEditAddField = new System.Windows.Forms.Button();
			this.txtNewName = new System.Windows.Forms.TextBox();
			this.lblNewFieldName = new System.Windows.Forms.Label();
			this.lblNote = new System.Windows.Forms.Label();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.panelEdit = new System.Windows.Forms.Panel();
			this.lblType = new System.Windows.Forms.Label();
			this.separator = new System.Windows.Forms.Label();
			this.lstFields = new System.Windows.Forms.ListBox();
			this.txtContent = new System.Windows.Forms.TextBox();
			this.chTVF = new System.Windows.Forms.CheckBox();
			this.chTokenized = new System.Windows.Forms.CheckBox();
			this.chIndexed = new System.Windows.Forms.CheckBox();
			this.chStored = new System.Windows.Forms.CheckBox();
			this.btnDeleteField = new System.Windows.Forms.Button();
			this.txtBoost = new System.Windows.Forms.TextBox();
			this.lblBoost = new System.Windows.Forms.Label();
			this.cmbAnalyzers = new System.Windows.Forms.ComboBox();
			this.lblAnalyzer = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnReplace = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupDocNum.SuspendLayout();
			this.panelEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupDocNum
			// 
			this.groupDocNum.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.groupDocNum.Controls.AddRange(new System.Windows.Forms.Control[] {
																					  this.btnEditAddField,
																					  this.txtNewName,
																					  this.lblNewFieldName,
																					  this.lblNote,
																					  this.panelEdit,
																					  this.cmbAnalyzers,
																					  this.lblAnalyzer});
			this.groupDocNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.groupDocNum.Location = new System.Drawing.Point(8, 8);
			this.groupDocNum.Name = "groupDocNum";
			this.groupDocNum.Size = new System.Drawing.Size(448, 376);
			this.groupDocNum.TabIndex = 0;
			this.groupDocNum.TabStop = false;
			this.groupDocNum.Text = "Fields of Doc # ";
			// 
			// btnEditAddField
			// 
			this.btnEditAddField.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnEditAddField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.btnEditAddField.Location = new System.Drawing.Point(216, 344);
			this.btnEditAddField.Name = "btnEditAddField";
			this.btnEditAddField.TabIndex = 6;
			this.btnEditAddField.Text = "&Add";
			this.btnEditAddField.Click += new System.EventHandler(this.btnEditAddField_Click);
			// 
			// txtNewName
			// 
			this.txtNewName.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtNewName.Location = new System.Drawing.Point(96, 345);
			this.txtNewName.Name = "txtNewName";
			this.txtNewName.Size = new System.Drawing.Size(112, 20);
			this.txtNewName.TabIndex = 5;
			this.txtNewName.Text = "";
			// 
			// lblNewFieldName
			// 
			this.lblNewFieldName.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblNewFieldName.AutoSize = true;
			this.lblNewFieldName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblNewFieldName.Location = new System.Drawing.Point(8, 349);
			this.lblNewFieldName.Name = "lblNewFieldName";
			this.lblNewFieldName.Size = new System.Drawing.Size(85, 13);
			this.lblNewFieldName.TabIndex = 4;
			this.lblNewFieldName.Text = "&New field name:";
			// 
			// lblNote
			// 
			this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblNote.Image = ((System.Drawing.Bitmap)(resources.GetObject("lblNote.Image")));
			this.lblNote.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lblNote.ImageIndex = 0;
			this.lblNote.ImageList = this.imageList;
			this.lblNote.Location = new System.Drawing.Point(8, 16);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(312, 16);
			this.lblNote.TabIndex = 0;
			this.lblNote.Text = "NOTE: unstored fields are reconstructed, some errors may be present!";
			this.lblNote.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panelEdit
			// 
			this.panelEdit.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.panelEdit.Controls.AddRange(new System.Windows.Forms.Control[] {
																					this.lblType,
																					this.separator,
																					this.lstFields,
																					this.txtContent,
																					this.chTVF,
																					this.chTokenized,
																					this.chIndexed,
																					this.chStored,
																					this.btnDeleteField,
																					this.txtBoost,
																					this.lblBoost});
			this.panelEdit.Location = new System.Drawing.Point(8, 40);
			this.panelEdit.Name = "panelEdit";
			this.panelEdit.Size = new System.Drawing.Size(432, 264);
			this.panelEdit.TabIndex = 1;
			// 
			// lblType
			// 
			this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblType.Location = new System.Drawing.Point(88, 56);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(336, 13);
			this.lblType.TabIndex = 19;
			// 
			// separator
			// 
			this.separator.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.separator.Location = new System.Drawing.Point(88, 250);
			this.separator.Name = "separator";
			this.separator.Size = new System.Drawing.Size(344, 3);
			this.separator.TabIndex = 18;
			// 
			// lstFields
			// 
			this.lstFields.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left);
			this.lstFields.Name = "lstFields";
			this.lstFields.Size = new System.Drawing.Size(80, 251);
			this.lstFields.TabIndex = 0;
			this.lstFields.SelectedIndexChanged += new System.EventHandler(this.lstFields_SelectedIndexChanged);
			// 
			// txtContent
			// 
			this.txtContent.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.txtContent.Location = new System.Drawing.Point(88, 72);
			this.txtContent.Multiline = true;
			this.txtContent.Name = "txtContent";
			this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtContent.Size = new System.Drawing.Size(344, 136);
			this.txtContent.TabIndex = 7;
			this.txtContent.Text = "";
			// 
			// chTVF
			// 
			this.chTVF.Location = new System.Drawing.Point(296, 32);
			this.chTVF.Name = "chTVF";
			this.chTVF.Size = new System.Drawing.Size(136, 24);
			this.chTVF.TabIndex = 6;
			this.chTVF.Text = "Stored Term &Vector";
			// 
			// chTokenized
			// 
			this.chTokenized.Location = new System.Drawing.Point(216, 32);
			this.chTokenized.Name = "chTokenized";
			this.chTokenized.Size = new System.Drawing.Size(80, 24);
			this.chTokenized.TabIndex = 5;
			this.chTokenized.Text = "&Tokenized";
			// 
			// chIndexed
			// 
			this.chIndexed.Location = new System.Drawing.Point(144, 32);
			this.chIndexed.Name = "chIndexed";
			this.chIndexed.Size = new System.Drawing.Size(80, 24);
			this.chIndexed.TabIndex = 4;
			this.chIndexed.Text = "&Indexed";
			// 
			// chStored
			// 
			this.chStored.Location = new System.Drawing.Point(88, 32);
			this.chStored.Name = "chStored";
			this.chStored.Size = new System.Drawing.Size(64, 24);
			this.chStored.TabIndex = 3;
			this.chStored.Text = "&Stored";
			// 
			// btnDeleteField
			// 
			this.btnDeleteField.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnDeleteField.Image = ((System.Drawing.Bitmap)(resources.GetObject("btnDeleteField.Image")));
			this.btnDeleteField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDeleteField.ImageIndex = 1;
			this.btnDeleteField.ImageList = this.imageList;
			this.btnDeleteField.Location = new System.Drawing.Point(344, 216);
			this.btnDeleteField.Name = "btnDeleteField";
			this.btnDeleteField.Size = new System.Drawing.Size(88, 24);
			this.btnDeleteField.TabIndex = 8;
			this.btnDeleteField.Text = "&Delete Field";
			this.btnDeleteField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDeleteField.Click += new System.EventHandler(this.btnDeleteField_Click);
			// 
			// txtBoost
			// 
			this.txtBoost.Location = new System.Drawing.Point(128, 4);
			this.txtBoost.Name = "txtBoost";
			this.txtBoost.Size = new System.Drawing.Size(40, 20);
			this.txtBoost.TabIndex = 2;
			this.txtBoost.Text = "";
			// 
			// lblBoost
			// 
			this.lblBoost.AutoSize = true;
			this.lblBoost.Location = new System.Drawing.Point(88, 8);
			this.lblBoost.Name = "lblBoost";
			this.lblBoost.Size = new System.Drawing.Size(36, 13);
			this.lblBoost.TabIndex = 1;
			this.lblBoost.Text = "&Boost:";
			// 
			// cmbAnalyzers
			// 
			this.cmbAnalyzers.Anchor = ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.cmbAnalyzers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAnalyzers.Location = new System.Drawing.Point(96, 312);
			this.cmbAnalyzers.Name = "cmbAnalyzers";
			this.cmbAnalyzers.Size = new System.Drawing.Size(248, 21);
			this.cmbAnalyzers.TabIndex = 3;
			// 
			// lblAnalyzer
			// 
			this.lblAnalyzer.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.lblAnalyzer.AutoSize = true;
			this.lblAnalyzer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.lblAnalyzer.Location = new System.Drawing.Point(8, 316);
			this.lblAnalyzer.Name = "lblAnalyzer";
			this.lblAnalyzer.Size = new System.Drawing.Size(73, 13);
			this.lblAnalyzer.TabIndex = 2;
			this.lblAnalyzer.Text = "&Use analyzer:";
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAdd.Location = new System.Drawing.Point(152, 400);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(104, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "&Add to index";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnReplace
			// 
			this.btnReplace.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnReplace.Location = new System.Drawing.Point(264, 400);
			this.btnReplace.Name = "btnReplace";
			this.btnReplace.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.btnReplace.Size = new System.Drawing.Size(104, 23);
			this.btnReplace.TabIndex = 2;
			this.btnReplace.Text = "&Delete old && Add";
			this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(376, 400);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			// 
			// EditDocument
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 437);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnCancel,
																		  this.btnReplace,
																		  this.btnAdd,
																		  this.groupDocNum});
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(472, 464);
			this.Name = "EditDocument";
			this.Text = " Edit Document";
			this.groupDocNum.ResumeLayout(false);
			this.panelEdit.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void PopulateFieldContent(int fieldIndex)
		{
			if (!(bool)fieldsReconstructed[fieldIndex])
			{
				lblType.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
				lblType.Text = "Original stored field content";
			}
			else
			{
				lblType.ForeColor = Color.Red;
				lblType.Text = "RESTORED content - check for errors!";
			}
			Field field = (Field)fields[fieldIndex];
			txtContent.Text = field.StringValue;
			txtBoost.Text = Lucene.Net.Util.Number.ToString(field.Boost);
			chStored.Checked = field.IsStored;
			chIndexed.Checked = field.IsIndexed;
			chTokenized.Checked = field.IsTokenized;
			chTVF.Checked = field.IsTermVectorStored;
		}

		private void ClearUI()
		{
			txtContent.Text = "";
			txtBoost.Text = "";
			chStored.Checked = false;
			chIndexed.Checked = false;
			chTokenized.Checked = false;
			chTVF.Checked = false;
		}

		private void SaveFieldContent(int fieldIndex)
		{
			if (fieldIndex > fields.Count - 1) return;

			float boost, 
				  oldBoost = ((Field)fields[fieldIndex]).Boost;
			Field field = new Field((string)lstFields.SelectedItem, 
						  txtContent.Text,
						  chStored.Checked?Field.Store.YES:Field.Store.NO,
						  chIndexed.Checked?Field.Index.ANALYZED:Field.Index.NOT_ANALYZED,
						  //chTokenized.Checked,
						  chTVF.Checked?Field.TermVector.YES:Field.TermVector.NO);
			try
			{
				boost = Single.Parse(txtBoost.Text, System.Globalization.NumberFormatInfo.InvariantInfo);
			}
			catch(Exception)
			{ boost = oldBoost; }
			field.Boost = boost;

			fields[fieldIndex] =  field;
		}
		private void lstFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// save changes
			if (lastIndex != lstFields.SelectedIndex && lstFields.SelectedIndex != -1)
			{
				SaveFieldContent(lastIndex);
				lastIndex = lstFields.SelectedIndex;

				PopulateFieldContent(lstFields.SelectedIndex);
			}
		}

		private void btnEditAddField_Click(object sender, System.EventArgs e)
		{
			string newName = txtNewName.Text.Trim();
			if (newName == "") 
			{
				luke.ShowStatus(resources.GetString("NoFieldName"));
				return;
			}
			int index;
			if ((index = lstFields.FindString(newName)) == -1)
			{
				lstFields.Items.Add(newName);
				fields.Add(new Field(newName, "",Field.Store.NO,Field.Index.NOT_ANALYZED_NO_NORMS));
				fieldsReconstructed.Add(false);

				lstFields.SelectedIndex = lstFields.Items.Count - 1;
			}
			else
			{
				// already exist
				lstFields.SelectedIndex = index;
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			EditAdd();
		}

		private void EditAdd()
		{
			Document doc = new Document();
			Analyzer a;

			string analyzerName = (string) cmbAnalyzers.SelectedItem;
			if (null == analyzerName || analyzerName == string.Empty) 
			{
				analyzerName = "Lucene.Net.Analysis.Standard.StandardAnalyzer";
				cmbAnalyzers.SelectedItem = analyzerName;
				a = new StandardAnalyzer(Util.Version.LUCENE_30);
			}
			else
			{
				a = luke.AnalyzerForName(analyzerName);
				
				if (null == a)
				{
					luke.ShowStatus(string.Format(resources.GetString("AnalyzerNotFound"), analyzerName));
					a = new StandardAnalyzer(Util.Version.LUCENE_30);
				}
			}
			
			foreach (Field f in fields) 
			{
				doc.Add(f);
			}

			IndexWriter writer = null;
			Directory dir = luke.IndexReader.Directory();
			try 
			{
				luke.IndexReader.Dispose();
				writer = new IndexWriter(dir, a, false, IndexWriter.MaxFieldLength.UNLIMITED);
				writer.AddDocument(doc);
			} 
			catch (Exception exc) 
			{
				luke.ErrorMessage(exc.Message);
			} 
			finally 
			{
				try 
				{
					if (writer != null) writer.Dispose();
				} 
				catch (Exception) 
				{
				}
				try 
				{
					luke.IndexReader = IndexReader.Open(dir, true);
				} 
				catch (Exception e2) 
				{
					luke.ErrorMessage(e2.Message);
				}
			}
		}

		private void btnReplace_Click(object sender, System.EventArgs e)
		{
			try
			{
			    luke.IndexReader.DeleteDocument(docNum);
			    //luke.IndexReader.Delete(docNum);
			} 
			catch (Exception exc) 
			{
				luke.ShowStatus(exc.Message);
			}
			EditAdd();
		}

		private void btnDeleteField_Click(object sender, System.EventArgs e)
		{
			if (lstFields.Items.Count > 0)
			{
				fields.RemoveAt(lstFields.SelectedIndex);
				int index = lstFields.SelectedIndex;
				lstFields.Items.RemoveAt(index);

				if (lstFields.Items.Count > 0)
				{
					if (index >= lstFields.Items.Count)
						lstFields.SelectedIndex = lstFields.Items.Count - 1;
					else
						lstFields.SelectedIndex = index;
				}
				else
					ClearUI();
			}
		}
	}
}
