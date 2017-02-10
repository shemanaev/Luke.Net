using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store;

using Lucene.Net.LukeNet.Plugins;
using System.Linq;

namespace Lucene.Net.LukeNet
{
    /// <summary>
    /// This class allows you to browse a <a href="jakarta.apache.org/lucene">Lucene</a>
    /// index in several ways - by document, by term, by query, and by most frequent terms.
    /// </summary>
    public class Luke : System.Windows.Forms.Form
    {
        #region Private UI Controls
        private System.Windows.Forms.MenuItem menuItemFile;
        private System.Windows.Forms.MenuItem menuItemSeparator;
        private System.Windows.Forms.MenuItem menuItemTools;
        private System.Windows.Forms.MenuItem menuItemHelp;
        private System.Windows.Forms.MenuItem menuItemAbout;
        private System.Windows.Forms.StatusBarPanel statusBarPanelIndex;
        private System.Windows.Forms.StatusBarPanel statusBarPanelMessage;
        private System.Windows.Forms.StatusBarPanel statusBarPanelLogo;
        private System.Windows.Forms.ColumnHeader columnHeaderNo;
        private System.Windows.Forms.ColumnHeader columnHeaderTopTermRank;
        private System.Windows.Forms.ColumnHeader columnHeaderTopTermField;
        private System.Windows.Forms.ColumnHeader columnHeaderTopTermText;
        private System.Windows.Forms.Label labelTopTerms;
        private System.Windows.Forms.Label labelSelectHint;
        private System.Windows.Forms.Label labelSelectHelp;
        private System.Windows.Forms.Label labelLastMod;
        private System.Windows.Forms.Label labelNumTerms;
        private System.Windows.Forms.Label labelNumDocs;
        private System.Windows.Forms.Label labelNumFields;
        private System.Windows.Forms.Label labelIndexName;
        private System.Windows.Forms.Label labelLegend;
        private System.Windows.Forms.Label labelDocTermFreq;
        private System.Windows.Forms.Label labelOf;
        private System.Windows.Forms.Label labelDoc;
        private System.Windows.Forms.Label labelTermDocFreq;
        private System.Windows.Forms.Label labelTerm;
        private System.Windows.Forms.Label labelBrowseHint;
        private System.Windows.Forms.Label labelZeroDoc;
        private System.Windows.Forms.Label labelBrowseDoc;
        private System.Windows.Forms.Label labelSearchResult;
        private System.Windows.Forms.Label labelSearchDocs;
        private System.Windows.Forms.ColumnHeader columnHeaderFieldName;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.MenuItem menuItemOptimize;
        private System.Windows.Forms.Label labelDocNum;
        private System.Windows.Forms.Label labelDocMax;
        private System.Windows.Forms.Label labelTermFreq;
        private System.Windows.Forms.Label labelSearchRes;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.StatusBar statusBar;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabOverview;
        private System.Windows.Forms.TabPage tabDocuments;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelMod;
        private System.Windows.Forms.Label labelTerms;
        private System.Windows.Forms.Label labelDocs;
        private System.Windows.Forms.Label labelFields;
        private System.Windows.Forms.Label labelListFields;
        private System.Windows.Forms.ListView listTerms;
        private System.Windows.Forms.ListView listFields;
        private System.Windows.Forms.ContextMenu contextMenu;
        private System.Windows.Forms.MenuItem contextMenuItemBrowse;
        private System.Windows.Forms.MenuItem contextMenuItemShowAll;
        private System.Windows.Forms.MenuItem contextMenuItemShowTV;
        private System.Windows.Forms.GroupBox groupDocNumber;
        private System.Windows.Forms.GroupBox groupTerm;
        private System.Windows.Forms.Button buttonPrevDoc;
        private System.Windows.Forms.TextBox textDocNum;
        private System.Windows.Forms.Button buttonNextDoc;
        private System.Windows.Forms.Label labelIndDocs;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonFirstTerm;
        private System.Windows.Forms.ComboBox comboTerms;
        private System.Windows.Forms.TextBox textTerm;
        private System.Windows.Forms.Button buttonNextTerm;
        private System.Windows.Forms.Label labelDocFreq;
        private System.Windows.Forms.Button buttonShowFirstDoc;
        private System.Windows.Forms.Button buttonShowNextDoc;
        private System.Windows.Forms.Button buttonShowAllDocs;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ColumnHeader columnHeaderField;
        private System.Windows.Forms.ColumnHeader columnHeaderIndexed;
        private System.Windows.Forms.ColumnHeader columnHeaderToken;
        private System.Windows.Forms.ColumnHeader columnHeaderStored;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        private System.Windows.Forms.Button buttonCopySelected;
        private System.Windows.Forms.Button buttonCopyAll;
        private System.Windows.Forms.Button buttonDeleteAllDocs;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ListView listDocFields;
        private System.Windows.Forms.Label labelCopy;
        private System.Windows.Forms.Label separatorOverview;
        private System.Windows.Forms.ColumnHeader columnHeaderRank;
        private System.Windows.Forms.ColumnHeader columnHeaderDocId;
        private System.Windows.Forms.ListView listSearch;
        private System.Windows.Forms.Button buttonSearchDelete;
        private System.Windows.Forms.MenuItem menuItemOpenIndex;
        private System.Windows.Forms.MenuItem menuItemUndelete;
        private System.Windows.Forms.MenuItem menuItemCompound;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.Label labelDeletionsTitle;
        private System.Windows.Forms.Label labelDeletions;
        private System.Windows.Forms.Label labelVersionTitle;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Button btnReconstruct;
        private System.Windows.Forms.Label labelInfoDocNumTitle;
        private System.Windows.Forms.Label labelInfoDocNum;
        private System.Windows.Forms.ColumnHeader columnHeaderTV;
        private System.Windows.Forms.Button btnTermVector;
        private System.Windows.Forms.Button btnExplain;
        private System.Windows.Forms.GroupBox groupSearchOptions;
        private System.Windows.Forms.Button btnUpdateParsedQuery;
        private System.Windows.Forms.Label labelParsedQuery;
        private System.Windows.Forms.ComboBox comboFields;
        private System.Windows.Forms.Label labelDefaultField;
        private System.Windows.Forms.ComboBox comboAnalyzer;
        private System.Windows.Forms.Label labelAnalyzer;
        private System.Windows.Forms.TextBox textSearch;
        private System.Windows.Forms.Label labelSearchExpr;
        private System.Windows.Forms.TabPage tabFiles;
        private System.Windows.Forms.Label labelIndexSize;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.ListView listIndexFiles;
        private System.Windows.Forms.ColumnHeader columnFilename;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnUnit;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.ListBox lstPlugins;
        private System.Windows.Forms.GroupBox groupPlugin;
        private System.Windows.Forms.GroupBox groupPluginInfo;
        private System.Windows.Forms.Label lblPluginInfo;
        private System.Windows.Forms.LinkLabel linkPluginURL;
        private System.Windows.Forms.Panel panelPlugin;
        #endregion Private UI Controls

        #region Fields
        private string lukeURL = "http://www.getopt.org/luke";

        private Progress progressDlg;
        private Query query;
        private int[] searchedDocIds;
        private string indexPath;
        private Preferences p;
        private ArrayList plugins = new ArrayList();
        private Document document;
        private Term term;
        private TermDocs termDocs;
        private bool readOnly;
        private FSDirectory dir;
        //private IndexSearcher searcher;
        private IndexReader indexReader;
        //private Hashtable fieldNames;
        private Analyzer stdAnalyzer = new StandardAnalyzer(Util.Version.LUCENE_30);
        private Analyzer analyzer;
        private QueryParser queryParser;
        private String[] indexFields;
        private bool useCompound;
        private int numTerms;
        private SortedList analyzers = new SortedList(); // Name -> Type
        private Type[] defaultAnalyzers =
            {
				typeof(Lucene.Net.Analysis.SimpleAnalyzer),
                typeof(Lucene.Net.Analysis.Standard.StandardAnalyzer),
                typeof(Lucene.Net.Analysis.StopAnalyzer),
                typeof(Lucene.Net.Analysis.WhitespaceAnalyzer)
            };
        private System.Windows.Forms.ColumnHeader columnHeaderBoost;
        private System.Windows.Forms.TextBox textParsed;
        private Panel panel1;
        private DomainUpDown domainTerms;
        private Label labelNumOfTerms;
        private Button buttonTopTerms;
        private ResourceManager resources = new ResourceManager
            (
                typeof(Luke).Namespace + ".Messages",
                Assembly.GetAssembly(typeof(Luke))
            );
        #endregion Fields

        #region Constructors
        public Luke()
        {
            InitializeComponent();

            p = new Preferences();
            p.Load();

            for (int i = 0; i <= 999999; i++)
                domainTerms.Items.Add(i);

            domainTerms.SelectedIndex = 50;

            SetOverviewContextMenuItems();
        }
        #endregion Constructors

        #region Cleanup 
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            if (indexReader != null)
                try
                {
                    indexReader.Dispose();
                }
                catch (Exception) { };

            if (dir != null)
                try
                {
                    dir.Dispose();
                }
                catch (Exception) { };

            if (p != null)
                try
                { p.Save(); }
                catch (Exception) { }

            base.Dispose(disposing);
        }
        #endregion Cleanup 

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Luke));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItemFile = new System.Windows.Forms.MenuItem();
            this.menuItemOpenIndex = new System.Windows.Forms.MenuItem();
            this.menuItemSeparator = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.menuItemTools = new System.Windows.Forms.MenuItem();
            this.menuItemUndelete = new System.Windows.Forms.MenuItem();
            this.menuItemOptimize = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItemCompound = new System.Windows.Forms.MenuItem();
            this.menuItemHelp = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanelIndex = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelMessage = new System.Windows.Forms.StatusBarPanel();
            this.statusBarPanelLogo = new System.Windows.Forms.StatusBarPanel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabOverview = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.domainTerms = new System.Windows.Forms.DomainUpDown();
            this.labelNumOfTerms = new System.Windows.Forms.Label();
            this.buttonTopTerms = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelVersionTitle = new System.Windows.Forms.Label();
            this.labelDeletions = new System.Windows.Forms.Label();
            this.labelDeletionsTitle = new System.Windows.Forms.Label();
            this.listFields = new System.Windows.Forms.ListView();
            this.columnHeaderFieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listTerms = new System.Windows.Forms.ListView();
            this.columnHeaderNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTopTermRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTopTermField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTopTermText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenu = new System.Windows.Forms.ContextMenu();
            this.labelTopTerms = new System.Windows.Forms.Label();
            this.labelListFields = new System.Windows.Forms.Label();
            this.labelSelectHint = new System.Windows.Forms.Label();
            this.labelSelectHelp = new System.Windows.Forms.Label();
            this.separatorOverview = new System.Windows.Forms.Label();
            this.labelFields = new System.Windows.Forms.Label();
            this.labelDocs = new System.Windows.Forms.Label();
            this.labelTerms = new System.Windows.Forms.Label();
            this.labelMod = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelLastMod = new System.Windows.Forms.Label();
            this.labelNumTerms = new System.Windows.Forms.Label();
            this.labelNumDocs = new System.Windows.Forms.Label();
            this.labelNumFields = new System.Windows.Forms.Label();
            this.labelIndexName = new System.Windows.Forms.Label();
            this.tabDocuments = new System.Windows.Forms.TabPage();
            this.btnTermVector = new System.Windows.Forms.Button();
            this.labelInfoDocNum = new System.Windows.Forms.Label();
            this.labelInfoDocNumTitle = new System.Windows.Forms.Label();
            this.labelCopy = new System.Windows.Forms.Label();
            this.buttonCopyAll = new System.Windows.Forms.Button();
            this.buttonCopySelected = new System.Windows.Forms.Button();
            this.labelLegend = new System.Windows.Forms.Label();
            this.listDocFields = new System.Windows.Forms.ListView();
            this.columnHeaderField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderIndexed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderToken = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStored = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderTV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderBoost = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupTerm = new System.Windows.Forms.GroupBox();
            this.labelTermFreq = new System.Windows.Forms.Label();
            this.labelDocTermFreq = new System.Windows.Forms.Label();
            this.labelDocMax = new System.Windows.Forms.Label();
            this.labelOf = new System.Windows.Forms.Label();
            this.labelDocNum = new System.Windows.Forms.Label();
            this.labelDoc = new System.Windows.Forms.Label();
            this.buttonDeleteAllDocs = new System.Windows.Forms.Button();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.buttonShowAllDocs = new System.Windows.Forms.Button();
            this.buttonShowNextDoc = new System.Windows.Forms.Button();
            this.buttonShowFirstDoc = new System.Windows.Forms.Button();
            this.labelDocFreq = new System.Windows.Forms.Label();
            this.labelTermDocFreq = new System.Windows.Forms.Label();
            this.buttonNextTerm = new System.Windows.Forms.Button();
            this.textTerm = new System.Windows.Forms.TextBox();
            this.comboTerms = new System.Windows.Forms.ComboBox();
            this.labelTerm = new System.Windows.Forms.Label();
            this.buttonFirstTerm = new System.Windows.Forms.Button();
            this.labelBrowseHint = new System.Windows.Forms.Label();
            this.groupDocNumber = new System.Windows.Forms.GroupBox();
            this.btnReconstruct = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelIndDocs = new System.Windows.Forms.Label();
            this.buttonNextDoc = new System.Windows.Forms.Button();
            this.textDocNum = new System.Windows.Forms.TextBox();
            this.buttonPrevDoc = new System.Windows.Forms.Button();
            this.labelZeroDoc = new System.Windows.Forms.Label();
            this.labelBrowseDoc = new System.Windows.Forms.Label();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.groupSearchOptions = new System.Windows.Forms.GroupBox();
            this.btnUpdateParsedQuery = new System.Windows.Forms.Button();
            this.textParsed = new System.Windows.Forms.TextBox();
            this.labelParsedQuery = new System.Windows.Forms.Label();
            this.comboFields = new System.Windows.Forms.ComboBox();
            this.labelDefaultField = new System.Windows.Forms.Label();
            this.comboAnalyzer = new System.Windows.Forms.ComboBox();
            this.labelAnalyzer = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textSearch = new System.Windows.Forms.TextBox();
            this.labelSearchExpr = new System.Windows.Forms.Label();
            this.btnExplain = new System.Windows.Forms.Button();
            this.labelSearchResult = new System.Windows.Forms.Label();
            this.labelSearchDocs = new System.Windows.Forms.Label();
            this.labelSearchRes = new System.Windows.Forms.Label();
            this.listSearch = new System.Windows.Forms.ListView();
            this.columnHeaderRank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDocId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonSearchDelete = new System.Windows.Forms.Button();
            this.tabFiles = new System.Windows.Forms.TabPage();
            this.listIndexFiles = new System.Windows.Forms.ListView();
            this.columnFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnUnit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFileSize = new System.Windows.Forms.Label();
            this.labelIndexSize = new System.Windows.Forms.Label();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.groupPlugin = new System.Windows.Forms.GroupBox();
            this.panelPlugin = new System.Windows.Forms.Panel();
            this.groupPluginInfo = new System.Windows.Forms.GroupBox();
            this.linkPluginURL = new System.Windows.Forms.LinkLabel();
            this.lblPluginInfo = new System.Windows.Forms.Label();
            this.lstPlugins = new System.Windows.Forms.ListBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelMessage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelLogo)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabOverview.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabDocuments.SuspendLayout();
            this.groupTerm.SuspendLayout();
            this.groupDocNumber.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.groupSearchOptions.SuspendLayout();
            this.tabFiles.SuspendLayout();
            this.tabPlugins.SuspendLayout();
            this.groupPlugin.SuspendLayout();
            this.groupPluginInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemTools,
            this.menuItemHelp});
            // 
            // menuItemFile
            // 
            this.menuItemFile.Index = 0;
            this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemOpenIndex,
            this.menuItemSeparator,
            this.menuItemExit});
            this.menuItemFile.Text = "&File";
            // 
            // menuItemOpenIndex
            // 
            this.menuItemOpenIndex.Index = 0;
            this.menuItemOpenIndex.Text = "&Open Lucene Index";
            this.menuItemOpenIndex.Click += new System.EventHandler(this.OnOpenIndex);
            // 
            // menuItemSeparator
            // 
            this.menuItemSeparator.Index = 1;
            this.menuItemSeparator.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 2;
            this.menuItemExit.Text = "&Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.Index = 1;
            this.menuItemTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemUndelete,
            this.menuItemOptimize,
            this.menuItem2,
            this.menuItemCompound});
            this.menuItemTools.Text = "&Tools";
            // 
            // menuItemUndelete
            // 
            this.menuItemUndelete.Index = 0;
            this.menuItemUndelete.Text = "&Undelete All";
            this.menuItemUndelete.Click += new System.EventHandler(this.menuItemUndelete_Click);
            // 
            // menuItemOptimize
            // 
            this.menuItemOptimize.Index = 1;
            this.menuItemOptimize.Text = "&Optimize Index";
            this.menuItemOptimize.Click += new System.EventHandler(this.menuItemOptimize_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "-";
            // 
            // menuItemCompound
            // 
            this.menuItemCompound.Checked = true;
            this.menuItemCompound.Index = 3;
            this.menuItemCompound.Text = "Use &Compound Files";
            this.menuItemCompound.Click += new System.EventHandler(this.menuItemCompound_Click);
            // 
            // menuItemHelp
            // 
            this.menuItemHelp.Index = 2;
            this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItemHelp.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "&About...";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 543);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanelIndex,
            this.statusBarPanelMessage,
            this.statusBarPanelLogo});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(760, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 0;
            this.statusBar.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.statusBar_PanelClick);
            // 
            // statusBarPanelIndex
            // 
            this.statusBarPanelIndex.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelIndex.Name = "statusBarPanelIndex";
            this.statusBarPanelIndex.Text = "Index name: ?";
            this.statusBarPanelIndex.Width = 86;
            // 
            // statusBarPanelMessage
            // 
            this.statusBarPanelMessage.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanelMessage.MinWidth = 150;
            this.statusBarPanelMessage.Name = "statusBarPanelMessage";
            this.statusBarPanelMessage.Width = 643;
            // 
            // statusBarPanelLogo
            // 
            this.statusBarPanelLogo.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.statusBarPanelLogo.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanelLogo.Icon = ((System.Drawing.Icon)(resources.GetObject("statusBarPanelLogo.Icon")));
            this.statusBarPanelLogo.MinWidth = 5;
            this.statusBarPanelLogo.Name = "statusBarPanelLogo";
            this.statusBarPanelLogo.ToolTipText = "Go to Luke homepage";
            this.statusBarPanelLogo.Width = 31;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabOverview);
            this.tabControl.Controls.Add(this.tabDocuments);
            this.tabControl.Controls.Add(this.tabSearch);
            this.tabControl.Controls.Add(this.tabFiles);
            this.tabControl.Controls.Add(this.tabPlugins);
            this.tabControl.ImageList = this.imageList;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 536);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabOverview
            // 
            this.tabOverview.Controls.Add(this.panel1);
            this.tabOverview.Controls.Add(this.labelVersion);
            this.tabOverview.Controls.Add(this.labelVersionTitle);
            this.tabOverview.Controls.Add(this.labelDeletions);
            this.tabOverview.Controls.Add(this.labelDeletionsTitle);
            this.tabOverview.Controls.Add(this.listFields);
            this.tabOverview.Controls.Add(this.listTerms);
            this.tabOverview.Controls.Add(this.labelTopTerms);
            this.tabOverview.Controls.Add(this.labelListFields);
            this.tabOverview.Controls.Add(this.labelSelectHint);
            this.tabOverview.Controls.Add(this.labelSelectHelp);
            this.tabOverview.Controls.Add(this.separatorOverview);
            this.tabOverview.Controls.Add(this.labelFields);
            this.tabOverview.Controls.Add(this.labelDocs);
            this.tabOverview.Controls.Add(this.labelTerms);
            this.tabOverview.Controls.Add(this.labelMod);
            this.tabOverview.Controls.Add(this.labelName);
            this.tabOverview.Controls.Add(this.labelLastMod);
            this.tabOverview.Controls.Add(this.labelNumTerms);
            this.tabOverview.Controls.Add(this.labelNumDocs);
            this.tabOverview.Controls.Add(this.labelNumFields);
            this.tabOverview.Controls.Add(this.labelIndexName);
            this.tabOverview.ImageIndex = 0;
            this.tabOverview.Location = new System.Drawing.Point(4, 23);
            this.tabOverview.Name = "tabOverview";
            this.tabOverview.Size = new System.Drawing.Size(752, 509);
            this.tabOverview.TabIndex = 0;
            this.tabOverview.Text = "Overview";
            this.tabOverview.Resize += new System.EventHandler(this.tabOverview_Resize);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.Controls.Add(this.domainTerms);
            this.panel1.Controls.Add(this.labelNumOfTerms);
            this.panel1.Controls.Add(this.buttonTopTerms);
            this.panel1.Location = new System.Drawing.Point(8, 447);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(171, 59);
            this.panel1.TabIndex = 25;
            // 
            // domainTerms
            // 
            this.domainTerms.Location = new System.Drawing.Point(110, 32);
            this.domainTerms.Name = "domainTerms";
            this.domainTerms.Size = new System.Drawing.Size(56, 20);
            this.domainTerms.TabIndex = 20;
            // 
            // labelNumOfTerms
            // 
            this.labelNumOfTerms.AutoSize = true;
            this.labelNumOfTerms.Location = new System.Drawing.Point(1, 36);
            this.labelNumOfTerms.Name = "labelNumOfTerms";
            this.labelNumOfTerms.Size = new System.Drawing.Size(105, 13);
            this.labelNumOfTerms.TabIndex = 19;
            this.labelNumOfTerms.Text = "&Number of top terms:";
            // 
            // buttonTopTerms
            // 
            this.buttonTopTerms.Location = new System.Drawing.Point(0, 3);
            this.buttonTopTerms.Name = "buttonTopTerms";
            this.buttonTopTerms.Size = new System.Drawing.Size(168, 23);
            this.buttonTopTerms.TabIndex = 18;
            this.buttonTopTerms.Text = "&Show top terms ->";
            this.buttonTopTerms.Click += new System.EventHandler(this.buttonTopTerms_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelVersion.Location = new System.Drawing.Point(128, 88);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(496, 16);
            this.labelVersion.TabIndex = 23;
            this.labelVersion.Text = "?";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVersionTitle
            // 
            this.labelVersionTitle.AutoSize = true;
            this.labelVersionTitle.Location = new System.Drawing.Point(51, 88);
            this.labelVersionTitle.Name = "labelVersionTitle";
            this.labelVersionTitle.Size = new System.Drawing.Size(73, 13);
            this.labelVersionTitle.TabIndex = 22;
            this.labelVersionTitle.Text = "Index version:";
            this.labelVersionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDeletions
            // 
            this.labelDeletions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDeletions.Location = new System.Drawing.Point(128, 72);
            this.labelDeletions.Name = "labelDeletions";
            this.labelDeletions.Size = new System.Drawing.Size(496, 16);
            this.labelDeletions.TabIndex = 21;
            this.labelDeletions.Text = "?";
            this.labelDeletions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDeletionsTitle
            // 
            this.labelDeletionsTitle.AutoSize = true;
            this.labelDeletionsTitle.Location = new System.Drawing.Point(50, 72);
            this.labelDeletionsTitle.Name = "labelDeletionsTitle";
            this.labelDeletionsTitle.Size = new System.Drawing.Size(74, 13);
            this.labelDeletionsTitle.TabIndex = 20;
            this.labelDeletionsTitle.Text = "Has deletions:";
            this.labelDeletionsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listFields
            // 
            this.listFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listFields.CheckBoxes = true;
            this.listFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderFieldName});
            this.listFields.FullRowSelect = true;
            this.listFields.GridLines = true;
            this.listFields.HideSelection = false;
            this.listFields.Location = new System.Drawing.Point(8, 184);
            this.listFields.MultiSelect = false;
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(168, 259);
            this.listFields.TabIndex = 14;
            this.listFields.UseCompatibleStateImageBehavior = false;
            this.listFields.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderFieldName
            // 
            this.columnHeaderFieldName.Text = "Name";
            this.columnHeaderFieldName.Width = 146;
            // 
            // listTerms
            // 
            this.listTerms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listTerms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderNo,
            this.columnHeaderTopTermRank,
            this.columnHeaderTopTermField,
            this.columnHeaderTopTermText});
            this.listTerms.ContextMenu = this.contextMenu;
            this.listTerms.FullRowSelect = true;
            this.listTerms.GridLines = true;
            this.listTerms.Location = new System.Drawing.Point(182, 184);
            this.listTerms.MultiSelect = false;
            this.listTerms.Name = "listTerms";
            this.listTerms.Size = new System.Drawing.Size(562, 317);
            this.listTerms.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listTerms.TabIndex = 19;
            this.listTerms.UseCompatibleStateImageBehavior = false;
            this.listTerms.View = System.Windows.Forms.View.Details;
            this.listTerms.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listTerms_ColumnClick);
            this.listTerms.DoubleClick += new System.EventHandler(this.listTerms_DoubleClick);
            // 
            // columnHeaderNo
            // 
            this.columnHeaderNo.Text = "?";
            this.columnHeaderNo.Width = 40;
            // 
            // columnHeaderTopTermRank
            // 
            this.columnHeaderTopTermRank.Text = "Rank";
            this.columnHeaderTopTermRank.Width = 50;
            // 
            // columnHeaderTopTermField
            // 
            this.columnHeaderTopTermField.Text = "Field";
            this.columnHeaderTopTermField.Width = 120;
            // 
            // columnHeaderTopTermText
            // 
            this.columnHeaderTopTermText.Text = "Text";
            this.columnHeaderTopTermText.Width = 306;
            // 
            // labelTopTerms
            // 
            this.labelTopTerms.AutoSize = true;
            this.labelTopTerms.Location = new System.Drawing.Point(297, 168);
            this.labelTopTerms.Name = "labelTopTerms";
            this.labelTopTerms.Size = new System.Drawing.Size(232, 13);
            this.labelTopTerms.TabIndex = 18;
            this.labelTopTerms.Text = "&Top ranking terms. (Right-click for more options)";
            // 
            // labelListFields
            // 
            this.labelListFields.AutoSize = true;
            this.labelListFields.Location = new System.Drawing.Point(8, 168);
            this.labelListFields.Name = "labelListFields";
            this.labelListFields.Size = new System.Drawing.Size(83, 13);
            this.labelListFields.TabIndex = 13;
            this.labelListFields.Text = "&Available Fields:";
            // 
            // labelSelectHint
            // 
            this.labelSelectHint.AutoSize = true;
            this.labelSelectHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSelectHint.Location = new System.Drawing.Point(8, 152);
            this.labelSelectHint.Name = "labelSelectHint";
            this.labelSelectHint.Size = new System.Drawing.Size(370, 12);
            this.labelSelectHint.TabIndex = 12;
            this.labelSelectHint.Text = "Hint: use Shift-Click to select ranges, or Ctrl-Click to select multiple fields (" +
    "or unselect all).";
            // 
            // labelSelectHelp
            // 
            this.labelSelectHelp.AutoSize = true;
            this.labelSelectHelp.Location = new System.Drawing.Point(8, 136);
            this.labelSelectHelp.Name = "labelSelectHelp";
            this.labelSelectHelp.Size = new System.Drawing.Size(528, 13);
            this.labelSelectHelp.TabIndex = 11;
            this.labelSelectHelp.Text = "Select fields from the list below, and press button to view top terms in these fi" +
    "elds. No selection means all fields.";
            // 
            // separatorOverview
            // 
            this.separatorOverview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorOverview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorOverview.Location = new System.Drawing.Point(8, 128);
            this.separatorOverview.Name = "separatorOverview";
            this.separatorOverview.Size = new System.Drawing.Size(736, 3);
            this.separatorOverview.TabIndex = 10;
            // 
            // labelFields
            // 
            this.labelFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelFields.Location = new System.Drawing.Point(128, 24);
            this.labelFields.Name = "labelFields";
            this.labelFields.Size = new System.Drawing.Size(496, 16);
            this.labelFields.TabIndex = 9;
            this.labelFields.Text = "?";
            this.labelFields.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelDocs
            // 
            this.labelDocs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDocs.Location = new System.Drawing.Point(128, 40);
            this.labelDocs.Name = "labelDocs";
            this.labelDocs.Size = new System.Drawing.Size(496, 16);
            this.labelDocs.TabIndex = 8;
            this.labelDocs.Text = "?";
            this.labelDocs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTerms
            // 
            this.labelTerms.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTerms.Location = new System.Drawing.Point(128, 56);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(496, 16);
            this.labelTerms.TabIndex = 7;
            this.labelTerms.Text = "?";
            this.labelTerms.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMod
            // 
            this.labelMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelMod.Location = new System.Drawing.Point(128, 104);
            this.labelMod.Name = "labelMod";
            this.labelMod.Size = new System.Drawing.Size(496, 16);
            this.labelMod.TabIndex = 6;
            this.labelMod.Text = "?";
            this.labelMod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelName.Location = new System.Drawing.Point(128, 8);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(496, 16);
            this.labelName.TabIndex = 5;
            this.labelName.Text = "?";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelLastMod
            // 
            this.labelLastMod.AutoSize = true;
            this.labelLastMod.Location = new System.Drawing.Point(51, 104);
            this.labelLastMod.Name = "labelLastMod";
            this.labelLastMod.Size = new System.Drawing.Size(72, 13);
            this.labelLastMod.TabIndex = 4;
            this.labelLastMod.Text = "Last modified:";
            this.labelLastMod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNumTerms
            // 
            this.labelNumTerms.AutoSize = true;
            this.labelNumTerms.Location = new System.Drawing.Point(35, 56);
            this.labelNumTerms.Name = "labelNumTerms";
            this.labelNumTerms.Size = new System.Drawing.Size(87, 13);
            this.labelNumTerms.TabIndex = 3;
            this.labelNumTerms.Text = "Number of terms:";
            this.labelNumTerms.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNumDocs
            // 
            this.labelNumDocs.AutoSize = true;
            this.labelNumDocs.Location = new System.Drawing.Point(8, 40);
            this.labelNumDocs.Name = "labelNumDocs";
            this.labelNumDocs.Size = new System.Drawing.Size(114, 13);
            this.labelNumDocs.TabIndex = 2;
            this.labelNumDocs.Text = "Number of documents:";
            this.labelNumDocs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNumFields
            // 
            this.labelNumFields.AutoSize = true;
            this.labelNumFields.Location = new System.Drawing.Point(36, 24);
            this.labelNumFields.Name = "labelNumFields";
            this.labelNumFields.Size = new System.Drawing.Size(86, 13);
            this.labelNumFields.TabIndex = 1;
            this.labelNumFields.Text = "Number of fields:";
            this.labelNumFields.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelIndexName
            // 
            this.labelIndexName.AutoSize = true;
            this.labelIndexName.Location = new System.Drawing.Point(60, 8);
            this.labelIndexName.Name = "labelIndexName";
            this.labelIndexName.Size = new System.Drawing.Size(65, 13);
            this.labelIndexName.TabIndex = 0;
            this.labelIndexName.Text = "Index name:";
            this.labelIndexName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabDocuments
            // 
            this.tabDocuments.Controls.Add(this.btnTermVector);
            this.tabDocuments.Controls.Add(this.labelInfoDocNum);
            this.tabDocuments.Controls.Add(this.labelInfoDocNumTitle);
            this.tabDocuments.Controls.Add(this.labelCopy);
            this.tabDocuments.Controls.Add(this.buttonCopyAll);
            this.tabDocuments.Controls.Add(this.buttonCopySelected);
            this.tabDocuments.Controls.Add(this.labelLegend);
            this.tabDocuments.Controls.Add(this.listDocFields);
            this.tabDocuments.Controls.Add(this.groupTerm);
            this.tabDocuments.Controls.Add(this.groupDocNumber);
            this.tabDocuments.ImageIndex = 1;
            this.tabDocuments.Location = new System.Drawing.Point(4, 23);
            this.tabDocuments.Name = "tabDocuments";
            this.tabDocuments.Size = new System.Drawing.Size(752, 509);
            this.tabDocuments.TabIndex = 1;
            this.tabDocuments.Text = "Documents";
            this.tabDocuments.Resize += new System.EventHandler(this.tabDocuments_Resize);
            // 
            // btnTermVector
            // 
            this.btnTermVector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTermVector.Location = new System.Drawing.Point(8, 477);
            this.btnTermVector.Name = "btnTermVector";
            this.btnTermVector.Size = new System.Drawing.Size(128, 23);
            this.btnTermVector.TabIndex = 3;
            this.btnTermVector.Text = "Field\'s Term &Vector";
            this.toolTip.SetToolTip(this.btnTermVector, "Show Term Vector of selected field");
            this.btnTermVector.Click += new System.EventHandler(this.btnTermVector_Click);
            // 
            // labelInfoDocNum
            // 
            this.labelInfoDocNum.Location = new System.Drawing.Point(56, 152);
            this.labelInfoDocNum.Name = "labelInfoDocNum";
            this.labelInfoDocNum.Size = new System.Drawing.Size(336, 13);
            this.labelInfoDocNum.TabIndex = 8;
            // 
            // labelInfoDocNumTitle
            // 
            this.labelInfoDocNumTitle.AutoSize = true;
            this.labelInfoDocNumTitle.Location = new System.Drawing.Point(16, 150);
            this.labelInfoDocNumTitle.Name = "labelInfoDocNumTitle";
            this.labelInfoDocNumTitle.Size = new System.Drawing.Size(37, 13);
            this.labelInfoDocNumTitle.TabIndex = 7;
            this.labelInfoDocNumTitle.Text = "Doc #";
            // 
            // labelCopy
            // 
            this.labelCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCopy.AutoSize = true;
            this.labelCopy.Location = new System.Drawing.Point(379, 482);
            this.labelCopy.Name = "labelCopy";
            this.labelCopy.Size = new System.Drawing.Size(113, 13);
            this.labelCopy.TabIndex = 6;
            this.labelCopy.Text = "Copy text to Clipboard:";
            // 
            // buttonCopyAll
            // 
            this.buttonCopyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopyAll.Location = new System.Drawing.Point(623, 477);
            this.buttonCopyAll.Name = "buttonCopyAll";
            this.buttonCopyAll.Size = new System.Drawing.Size(120, 23);
            this.buttonCopyAll.TabIndex = 5;
            this.buttonCopyAll.Text = "C&omplete Document";
            this.toolTip.SetToolTip(this.buttonCopyAll, "Copy all fields to Clipboard");
            this.buttonCopyAll.Click += new System.EventHandler(this.buttonCopyAll_Click);
            // 
            // buttonCopySelected
            // 
            this.buttonCopySelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCopySelected.Location = new System.Drawing.Point(499, 477);
            this.buttonCopySelected.Name = "buttonCopySelected";
            this.buttonCopySelected.Size = new System.Drawing.Size(120, 23);
            this.buttonCopySelected.TabIndex = 4;
            this.buttonCopySelected.Text = "Se&lected Fields";
            this.toolTip.SetToolTip(this.buttonCopySelected, "Copy selected fields to Clipboard");
            this.buttonCopySelected.Click += new System.EventHandler(this.buttonCopySelected_Click);
            // 
            // labelLegend
            // 
            this.labelLegend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLegend.AutoSize = true;
            this.labelLegend.Location = new System.Drawing.Point(424, 150);
            this.labelLegend.Name = "labelLegend";
            this.labelLegend.Size = new System.Drawing.Size(304, 13);
            this.labelLegend.TabIndex = 3;
            this.labelLegend.Text = "Legend: I - Indexed; T - Tokenized; S - Stored, V - Term Vector";
            // 
            // listDocFields
            // 
            this.listDocFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listDocFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderField,
            this.columnHeaderIndexed,
            this.columnHeaderToken,
            this.columnHeaderStored,
            this.columnHeaderTV,
            this.columnHeaderBoost,
            this.columnHeaderValue});
            this.listDocFields.ContextMenu = this.contextMenu;
            this.listDocFields.FullRowSelect = true;
            this.listDocFields.GridLines = true;
            this.listDocFields.Location = new System.Drawing.Point(8, 168);
            this.listDocFields.MultiSelect = false;
            this.listDocFields.Name = "listDocFields";
            this.listDocFields.Size = new System.Drawing.Size(736, 301);
            this.listDocFields.TabIndex = 2;
            this.listDocFields.UseCompatibleStateImageBehavior = false;
            this.listDocFields.View = System.Windows.Forms.View.Details;
            this.listDocFields.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listDocFields_ColumnClick);
            // 
            // columnHeaderField
            // 
            this.columnHeaderField.Text = "Field";
            this.columnHeaderField.Width = 164;
            // 
            // columnHeaderIndexed
            // 
            this.columnHeaderIndexed.Text = " I";
            this.columnHeaderIndexed.Width = 20;
            // 
            // columnHeaderToken
            // 
            this.columnHeaderToken.Text = "T";
            this.columnHeaderToken.Width = 20;
            // 
            // columnHeaderStored
            // 
            this.columnHeaderStored.Text = "S";
            this.columnHeaderStored.Width = 20;
            // 
            // columnHeaderTV
            // 
            this.columnHeaderTV.Text = "V";
            this.columnHeaderTV.Width = 20;
            // 
            // columnHeaderBoost
            // 
            this.columnHeaderBoost.Text = "Boost";
            this.columnHeaderBoost.Width = 40;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "String Value";
            this.columnHeaderValue.Width = 532;
            // 
            // groupTerm
            // 
            this.groupTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTerm.Controls.Add(this.labelTermFreq);
            this.groupTerm.Controls.Add(this.labelDocTermFreq);
            this.groupTerm.Controls.Add(this.labelDocMax);
            this.groupTerm.Controls.Add(this.labelOf);
            this.groupTerm.Controls.Add(this.labelDocNum);
            this.groupTerm.Controls.Add(this.labelDoc);
            this.groupTerm.Controls.Add(this.buttonDeleteAllDocs);
            this.groupTerm.Controls.Add(this.buttonShowAllDocs);
            this.groupTerm.Controls.Add(this.buttonShowNextDoc);
            this.groupTerm.Controls.Add(this.buttonShowFirstDoc);
            this.groupTerm.Controls.Add(this.labelDocFreq);
            this.groupTerm.Controls.Add(this.labelTermDocFreq);
            this.groupTerm.Controls.Add(this.buttonNextTerm);
            this.groupTerm.Controls.Add(this.textTerm);
            this.groupTerm.Controls.Add(this.comboTerms);
            this.groupTerm.Controls.Add(this.labelTerm);
            this.groupTerm.Controls.Add(this.buttonFirstTerm);
            this.groupTerm.Controls.Add(this.labelBrowseHint);
            this.groupTerm.Location = new System.Drawing.Point(224, 8);
            this.groupTerm.Name = "groupTerm";
            this.groupTerm.Size = new System.Drawing.Size(520, 136);
            this.groupTerm.TabIndex = 1;
            this.groupTerm.TabStop = false;
            this.groupTerm.Text = "Browse by term";
            // 
            // labelTermFreq
            // 
            this.labelTermFreq.AutoSize = true;
            this.labelTermFreq.Location = new System.Drawing.Point(360, 111);
            this.labelTermFreq.Name = "labelTermFreq";
            this.labelTermFreq.Size = new System.Drawing.Size(13, 13);
            this.labelTermFreq.TabIndex = 17;
            this.labelTermFreq.Text = "?";
            // 
            // labelDocTermFreq
            // 
            this.labelDocTermFreq.AutoSize = true;
            this.labelDocTermFreq.Location = new System.Drawing.Point(248, 111);
            this.labelDocTermFreq.Name = "labelDocTermFreq";
            this.labelDocTermFreq.Size = new System.Drawing.Size(106, 13);
            this.labelDocTermFreq.TabIndex = 16;
            this.labelDocTermFreq.Text = "Term freq in this doc:";
            // 
            // labelDocMax
            // 
            this.labelDocMax.AutoSize = true;
            this.labelDocMax.Location = new System.Drawing.Point(360, 85);
            this.labelDocMax.Name = "labelDocMax";
            this.labelDocMax.Size = new System.Drawing.Size(13, 13);
            this.labelDocMax.TabIndex = 15;
            this.labelDocMax.Text = "?";
            // 
            // labelOf
            // 
            this.labelOf.AutoSize = true;
            this.labelOf.Location = new System.Drawing.Point(336, 85);
            this.labelOf.Name = "labelOf";
            this.labelOf.Size = new System.Drawing.Size(16, 13);
            this.labelOf.TabIndex = 14;
            this.labelOf.Text = "of";
            // 
            // labelDocNum
            // 
            this.labelDocNum.AutoSize = true;
            this.labelDocNum.Location = new System.Drawing.Point(320, 85);
            this.labelDocNum.Name = "labelDocNum";
            this.labelDocNum.Size = new System.Drawing.Size(13, 13);
            this.labelDocNum.TabIndex = 13;
            this.labelDocNum.Text = "?";
            // 
            // labelDoc
            // 
            this.labelDoc.AutoSize = true;
            this.labelDoc.Location = new System.Drawing.Point(248, 85);
            this.labelDoc.Name = "labelDoc";
            this.labelDoc.Size = new System.Drawing.Size(59, 13);
            this.labelDoc.TabIndex = 12;
            this.labelDoc.Text = "Document:";
            // 
            // buttonDeleteAllDocs
            // 
            this.buttonDeleteAllDocs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDeleteAllDocs.ImageIndex = 3;
            this.buttonDeleteAllDocs.ImageList = this.imageList;
            this.buttonDeleteAllDocs.Location = new System.Drawing.Point(123, 106);
            this.buttonDeleteAllDocs.Name = "buttonDeleteAllDocs";
            this.buttonDeleteAllDocs.Size = new System.Drawing.Size(112, 23);
            this.buttonDeleteAllDocs.TabIndex = 11;
            this.buttonDeleteAllDocs.Text = "Delete &All Docs";
            this.buttonDeleteAllDocs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonDeleteAllDocs, "Delete all docs with this term (NO WARNING!)");
            this.buttonDeleteAllDocs.Click += new System.EventHandler(this.buttonDeleteAllDocs_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            this.imageList.Images.SetKeyName(1, "");
            this.imageList.Images.SetKeyName(2, "");
            this.imageList.Images.SetKeyName(3, "");
            this.imageList.Images.SetKeyName(4, "");
            this.imageList.Images.SetKeyName(5, "");
            // 
            // buttonShowAllDocs
            // 
            this.buttonShowAllDocs.Location = new System.Drawing.Point(8, 106);
            this.buttonShowAllDocs.Name = "buttonShowAllDocs";
            this.buttonShowAllDocs.Size = new System.Drawing.Size(112, 23);
            this.buttonShowAllDocs.TabIndex = 10;
            this.buttonShowAllDocs.Text = "&Show All Docs";
            this.toolTip.SetToolTip(this.buttonShowAllDocs, "Show all docs with this term");
            this.buttonShowAllDocs.Click += new System.EventHandler(this.buttonShowAllDocs_Click);
            // 
            // buttonShowNextDoc
            // 
            this.buttonShowNextDoc.Location = new System.Drawing.Point(123, 80);
            this.buttonShowNextDoc.Name = "buttonShowNextDoc";
            this.buttonShowNextDoc.Size = new System.Drawing.Size(112, 23);
            this.buttonShowNextDoc.TabIndex = 9;
            this.buttonShowNextDoc.Text = "N&ext Doc ->";
            this.buttonShowNextDoc.Click += new System.EventHandler(this.buttonShowNextDoc_Click);
            // 
            // buttonShowFirstDoc
            // 
            this.buttonShowFirstDoc.Location = new System.Drawing.Point(8, 80);
            this.buttonShowFirstDoc.Name = "buttonShowFirstDoc";
            this.buttonShowFirstDoc.Size = new System.Drawing.Size(112, 23);
            this.buttonShowFirstDoc.TabIndex = 8;
            this.buttonShowFirstDoc.Text = "Fi&rst Doc";
            this.buttonShowFirstDoc.Click += new System.EventHandler(this.buttonShowFirstDoc_Click);
            // 
            // labelDocFreq
            // 
            this.labelDocFreq.AutoSize = true;
            this.labelDocFreq.Location = new System.Drawing.Point(112, 64);
            this.labelDocFreq.Name = "labelDocFreq";
            this.labelDocFreq.Size = new System.Drawing.Size(13, 13);
            this.labelDocFreq.TabIndex = 7;
            this.labelDocFreq.Text = "?";
            // 
            // labelTermDocFreq
            // 
            this.labelTermDocFreq.AutoSize = true;
            this.labelTermDocFreq.Location = new System.Drawing.Point(8, 64);
            this.labelTermDocFreq.Name = "labelTermDocFreq";
            this.labelTermDocFreq.Size = new System.Drawing.Size(105, 13);
            this.labelTermDocFreq.TabIndex = 6;
            this.labelTermDocFreq.Text = "Doc freq of this term:";
            // 
            // buttonNextTerm
            // 
            this.buttonNextTerm.Location = new System.Drawing.Point(363, 33);
            this.buttonNextTerm.Name = "buttonNextTerm";
            this.buttonNextTerm.Size = new System.Drawing.Size(80, 23);
            this.buttonNextTerm.TabIndex = 5;
            this.buttonNextTerm.Text = "&Next Term ->";
            this.buttonNextTerm.Click += new System.EventHandler(this.buttonNextTerm_Click);
            // 
            // textTerm
            // 
            this.textTerm.Location = new System.Drawing.Point(273, 34);
            this.textTerm.Name = "textTerm";
            this.textTerm.Size = new System.Drawing.Size(88, 20);
            this.textTerm.TabIndex = 4;
            this.textTerm.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTerm_KeyPress);
            // 
            // comboTerms
            // 
            this.comboTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboTerms.Location = new System.Drawing.Point(120, 33);
            this.comboTerms.Name = "comboTerms";
            this.comboTerms.Size = new System.Drawing.Size(148, 21);
            this.comboTerms.TabIndex = 3;
            // 
            // labelTerm
            // 
            this.labelTerm.AutoSize = true;
            this.labelTerm.Location = new System.Drawing.Point(88, 37);
            this.labelTerm.Name = "labelTerm";
            this.labelTerm.Size = new System.Drawing.Size(34, 13);
            this.labelTerm.TabIndex = 2;
            this.labelTerm.Text = "T&erm:";
            // 
            // buttonFirstTerm
            // 
            this.buttonFirstTerm.Location = new System.Drawing.Point(8, 32);
            this.buttonFirstTerm.Name = "buttonFirstTerm";
            this.buttonFirstTerm.Size = new System.Drawing.Size(75, 23);
            this.buttonFirstTerm.TabIndex = 1;
            this.buttonFirstTerm.Text = "F&irst Term";
            this.buttonFirstTerm.Click += new System.EventHandler(this.buttonFirstTerm_Click);
            // 
            // labelBrowseHint
            // 
            this.labelBrowseHint.AutoSize = true;
            this.labelBrowseHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelBrowseHint.Location = new System.Drawing.Point(8, 16);
            this.labelBrowseHint.Name = "labelBrowseHint";
            this.labelBrowseHint.Size = new System.Drawing.Size(278, 12);
            this.labelBrowseHint.TabIndex = 0;
            this.labelBrowseHint.Text = "(Hint: enter a substring and press Next to start at the nearest term).";
            // 
            // groupDocNumber
            // 
            this.groupDocNumber.Controls.Add(this.btnReconstruct);
            this.groupDocNumber.Controls.Add(this.buttonDelete);
            this.groupDocNumber.Controls.Add(this.labelIndDocs);
            this.groupDocNumber.Controls.Add(this.buttonNextDoc);
            this.groupDocNumber.Controls.Add(this.textDocNum);
            this.groupDocNumber.Controls.Add(this.buttonPrevDoc);
            this.groupDocNumber.Controls.Add(this.labelZeroDoc);
            this.groupDocNumber.Controls.Add(this.labelBrowseDoc);
            this.groupDocNumber.Location = new System.Drawing.Point(8, 8);
            this.groupDocNumber.Name = "groupDocNumber";
            this.groupDocNumber.Size = new System.Drawing.Size(208, 136);
            this.groupDocNumber.TabIndex = 0;
            this.groupDocNumber.TabStop = false;
            this.groupDocNumber.Text = "Browse by doc. number";
            // 
            // btnReconstruct
            // 
            this.btnReconstruct.Location = new System.Drawing.Point(8, 104);
            this.btnReconstruct.Name = "btnReconstruct";
            this.btnReconstruct.Size = new System.Drawing.Size(112, 23);
            this.btnReconstruct.TabIndex = 6;
            this.btnReconstruct.Text = "&Reconstruct && Edit";
            this.toolTip.SetToolTip(this.btnReconstruct, "Reconstruct all field contents &amp; edit doc");
            this.btnReconstruct.Click += new System.EventHandler(this.btnReconstruct_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonDelete.ImageIndex = 3;
            this.buttonDelete.ImageList = this.imageList;
            this.buttonDelete.Location = new System.Drawing.Point(128, 104);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(72, 23);
            this.buttonDelete.TabIndex = 7;
            this.buttonDelete.Text = "&Delete";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete this document (NO WARNING)");
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelIndDocs
            // 
            this.labelIndDocs.AutoSize = true;
            this.labelIndDocs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelIndDocs.Location = new System.Drawing.Point(178, 37);
            this.labelIndDocs.Name = "labelIndDocs";
            this.labelIndDocs.Size = new System.Drawing.Size(14, 13);
            this.labelIndDocs.TabIndex = 5;
            this.labelIndDocs.Text = "?";
            // 
            // buttonNextDoc
            // 
            this.buttonNextDoc.Location = new System.Drawing.Point(148, 32);
            this.buttonNextDoc.Name = "buttonNextDoc";
            this.buttonNextDoc.Size = new System.Drawing.Size(24, 23);
            this.buttonNextDoc.TabIndex = 4;
            this.buttonNextDoc.Text = "->";
            this.buttonNextDoc.Click += new System.EventHandler(this.buttonNextDoc_Click);
            // 
            // textDocNum
            // 
            this.textDocNum.Location = new System.Drawing.Point(98, 33);
            this.textDocNum.Name = "textDocNum";
            this.textDocNum.Size = new System.Drawing.Size(48, 20);
            this.textDocNum.TabIndex = 3;
            this.textDocNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textDocNum_KeyPress);
            // 
            // buttonPrevDoc
            // 
            this.buttonPrevDoc.Location = new System.Drawing.Point(72, 32);
            this.buttonPrevDoc.Name = "buttonPrevDoc";
            this.buttonPrevDoc.Size = new System.Drawing.Size(24, 23);
            this.buttonPrevDoc.TabIndex = 2;
            this.buttonPrevDoc.Text = "<-";
            this.buttonPrevDoc.Click += new System.EventHandler(this.buttonPrevDoc_Click);
            // 
            // labelZeroDoc
            // 
            this.labelZeroDoc.AutoSize = true;
            this.labelZeroDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelZeroDoc.Location = new System.Drawing.Point(56, 37);
            this.labelZeroDoc.Name = "labelZeroDoc";
            this.labelZeroDoc.Size = new System.Drawing.Size(14, 13);
            this.labelZeroDoc.TabIndex = 1;
            this.labelZeroDoc.Text = "0";
            // 
            // labelBrowseDoc
            // 
            this.labelBrowseDoc.AutoSize = true;
            this.labelBrowseDoc.Location = new System.Drawing.Point(8, 37);
            this.labelBrowseDoc.Name = "labelBrowseDoc";
            this.labelBrowseDoc.Size = new System.Drawing.Size(43, 13);
            this.labelBrowseDoc.TabIndex = 0;
            this.labelBrowseDoc.Text = "Doc. #:";
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.groupSearchOptions);
            this.tabSearch.Controls.Add(this.btnExplain);
            this.tabSearch.Controls.Add(this.labelSearchResult);
            this.tabSearch.Controls.Add(this.labelSearchDocs);
            this.tabSearch.Controls.Add(this.labelSearchRes);
            this.tabSearch.Controls.Add(this.listSearch);
            this.tabSearch.Controls.Add(this.buttonSearchDelete);
            this.tabSearch.ImageIndex = 2;
            this.tabSearch.Location = new System.Drawing.Point(4, 23);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Size = new System.Drawing.Size(752, 509);
            this.tabSearch.TabIndex = 2;
            this.tabSearch.Text = "Search";
            // 
            // groupSearchOptions
            // 
            this.groupSearchOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSearchOptions.Controls.Add(this.btnUpdateParsedQuery);
            this.groupSearchOptions.Controls.Add(this.textParsed);
            this.groupSearchOptions.Controls.Add(this.labelParsedQuery);
            this.groupSearchOptions.Controls.Add(this.comboFields);
            this.groupSearchOptions.Controls.Add(this.labelDefaultField);
            this.groupSearchOptions.Controls.Add(this.comboAnalyzer);
            this.groupSearchOptions.Controls.Add(this.labelAnalyzer);
            this.groupSearchOptions.Controls.Add(this.buttonSearch);
            this.groupSearchOptions.Controls.Add(this.textSearch);
            this.groupSearchOptions.Controls.Add(this.labelSearchExpr);
            this.groupSearchOptions.Location = new System.Drawing.Point(8, 8);
            this.groupSearchOptions.Name = "groupSearchOptions";
            this.groupSearchOptions.Size = new System.Drawing.Size(736, 200);
            this.groupSearchOptions.TabIndex = 0;
            this.groupSearchOptions.TabStop = false;
            this.groupSearchOptions.Text = "Search expression";
            // 
            // btnUpdateParsedQuery
            // 
            this.btnUpdateParsedQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateParsedQuery.Location = new System.Drawing.Point(616, 168);
            this.btnUpdateParsedQuery.Name = "btnUpdateParsedQuery";
            this.btnUpdateParsedQuery.Size = new System.Drawing.Size(104, 23);
            this.btnUpdateParsedQuery.TabIndex = 8;
            this.btnUpdateParsedQuery.Text = "&Update";
            this.btnUpdateParsedQuery.Click += new System.EventHandler(this.btnUpdateParsedQuery_Click);
            // 
            // textParsed
            // 
            this.textParsed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textParsed.Location = new System.Drawing.Point(320, 97);
            this.textParsed.Multiline = true;
            this.textParsed.Name = "textParsed";
            this.textParsed.Size = new System.Drawing.Size(400, 64);
            this.textParsed.TabIndex = 7;
            this.textParsed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textParsed_KeyPress);
            // 
            // labelParsedQuery
            // 
            this.labelParsedQuery.AutoSize = true;
            this.labelParsedQuery.Location = new System.Drawing.Point(320, 81);
            this.labelParsedQuery.Name = "labelParsedQuery";
            this.labelParsedQuery.Size = new System.Drawing.Size(97, 13);
            this.labelParsedQuery.TabIndex = 6;
            this.labelParsedQuery.Text = "&Parsed query view:";
            // 
            // comboFields
            // 
            this.comboFields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFields.Location = new System.Drawing.Point(96, 49);
            this.comboFields.Name = "comboFields";
            this.comboFields.Size = new System.Drawing.Size(624, 21);
            this.comboFields.TabIndex = 3;
            // 
            // labelDefaultField
            // 
            this.labelDefaultField.AutoSize = true;
            this.labelDefaultField.Location = new System.Drawing.Point(16, 53);
            this.labelDefaultField.Name = "labelDefaultField";
            this.labelDefaultField.Size = new System.Drawing.Size(66, 13);
            this.labelDefaultField.TabIndex = 2;
            this.labelDefaultField.Text = "&Default field:";
            // 
            // comboAnalyzer
            // 
            this.comboAnalyzer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboAnalyzer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAnalyzer.Location = new System.Drawing.Point(96, 25);
            this.comboAnalyzer.Name = "comboAnalyzer";
            this.comboAnalyzer.Size = new System.Drawing.Size(624, 21);
            this.comboAnalyzer.TabIndex = 1;
            this.toolTip.SetToolTip(this.comboAnalyzer, "Analyzer to use for query parsing");
            // 
            // labelAnalyzer
            // 
            this.labelAnalyzer.AutoSize = true;
            this.labelAnalyzer.Location = new System.Drawing.Point(16, 29);
            this.labelAnalyzer.Name = "labelAnalyzer";
            this.labelAnalyzer.Size = new System.Drawing.Size(50, 13);
            this.labelAnalyzer.TabIndex = 0;
            this.labelAnalyzer.Text = "&Analyzer:";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(506, 168);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(104, 23);
            this.buttonSearch.TabIndex = 1;
            this.buttonSearch.Text = "&Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textSearch
            // 
            this.textSearch.Location = new System.Drawing.Point(16, 97);
            this.textSearch.Multiline = true;
            this.textSearch.Name = "textSearch";
            this.textSearch.Size = new System.Drawing.Size(288, 64);
            this.textSearch.TabIndex = 5;
            this.textSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textSearch_KeyDown);
            // 
            // labelSearchExpr
            // 
            this.labelSearchExpr.AutoSize = true;
            this.labelSearchExpr.Location = new System.Drawing.Point(16, 81);
            this.labelSearchExpr.Name = "labelSearchExpr";
            this.labelSearchExpr.Size = new System.Drawing.Size(97, 13);
            this.labelSearchExpr.TabIndex = 4;
            this.labelSearchExpr.Text = "S&earch expression:";
            // 
            // btnExplain
            // 
            this.btnExplain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExplain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExplain.ImageIndex = 0;
            this.btnExplain.ImageList = this.imageList;
            this.btnExplain.Location = new System.Drawing.Point(592, 477);
            this.btnExplain.Name = "btnExplain";
            this.btnExplain.Size = new System.Drawing.Size(75, 23);
            this.btnExplain.TabIndex = 3;
            this.btnExplain.Text = "E&xplain";
            this.btnExplain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExplain.Click += new System.EventHandler(this.btnExplain_Click);
            // 
            // labelSearchResult
            // 
            this.labelSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSearchResult.AutoSize = true;
            this.labelSearchResult.Location = new System.Drawing.Point(8, 482);
            this.labelSearchResult.Name = "labelSearchResult";
            this.labelSearchResult.Size = new System.Drawing.Size(77, 13);
            this.labelSearchResult.TabIndex = 8;
            this.labelSearchResult.Text = "Search Result:";
            // 
            // labelSearchDocs
            // 
            this.labelSearchDocs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSearchDocs.AutoSize = true;
            this.labelSearchDocs.Location = new System.Drawing.Point(128, 482);
            this.labelSearchDocs.Name = "labelSearchDocs";
            this.labelSearchDocs.Size = new System.Drawing.Size(38, 13);
            this.labelSearchDocs.TabIndex = 10;
            this.labelSearchDocs.Text = "Doc(s)";
            // 
            // labelSearchRes
            // 
            this.labelSearchRes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSearchRes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSearchRes.Location = new System.Drawing.Point(88, 482);
            this.labelSearchRes.Name = "labelSearchRes";
            this.labelSearchRes.Size = new System.Drawing.Size(32, 13);
            this.labelSearchRes.TabIndex = 9;
            this.labelSearchRes.Text = "0";
            this.labelSearchRes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // listSearch
            // 
            this.listSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSearch.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderRank,
            this.columnHeaderDocId});
            this.listSearch.FullRowSelect = true;
            this.listSearch.GridLines = true;
            this.listSearch.Location = new System.Drawing.Point(8, 216);
            this.listSearch.MultiSelect = false;
            this.listSearch.Name = "listSearch";
            this.listSearch.Size = new System.Drawing.Size(736, 253);
            this.listSearch.TabIndex = 2;
            this.toolTip.SetToolTip(this.listSearch, "Double-click on results to display all document fields");
            this.listSearch.UseCompatibleStateImageBehavior = false;
            this.listSearch.View = System.Windows.Forms.View.Details;
            this.listSearch.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listSearch_ColumnClick);
            this.listSearch.DoubleClick += new System.EventHandler(this.listSearch_DoubleClick);
            // 
            // columnHeaderRank
            // 
            this.columnHeaderRank.Text = "Rank %";
            this.columnHeaderRank.Width = 50;
            // 
            // columnHeaderDocId
            // 
            this.columnHeaderDocId.Text = "Doc. Id";
            // 
            // buttonSearchDelete
            // 
            this.buttonSearchDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearchDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonSearchDelete.ImageIndex = 3;
            this.buttonSearchDelete.ImageList = this.imageList;
            this.buttonSearchDelete.Location = new System.Drawing.Point(672, 477);
            this.buttonSearchDelete.Name = "buttonSearchDelete";
            this.buttonSearchDelete.Size = new System.Drawing.Size(72, 23);
            this.buttonSearchDelete.TabIndex = 4;
            this.buttonSearchDelete.Text = "D&elete";
            this.buttonSearchDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonSearchDelete, "Delete selected docs");
            this.buttonSearchDelete.Click += new System.EventHandler(this.buttonSearchDelete_Click);
            // 
            // tabFiles
            // 
            this.tabFiles.Controls.Add(this.listIndexFiles);
            this.tabFiles.Controls.Add(this.lblFileSize);
            this.tabFiles.Controls.Add(this.labelIndexSize);
            this.tabFiles.ImageIndex = 4;
            this.tabFiles.Location = new System.Drawing.Point(4, 23);
            this.tabFiles.Name = "tabFiles";
            this.tabFiles.Size = new System.Drawing.Size(752, 509);
            this.tabFiles.TabIndex = 3;
            this.tabFiles.Text = "Files";
            // 
            // listIndexFiles
            // 
            this.listIndexFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listIndexFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnFilename,
            this.columnSize,
            this.columnUnit});
            this.listIndexFiles.FullRowSelect = true;
            this.listIndexFiles.GridLines = true;
            this.listIndexFiles.Location = new System.Drawing.Point(8, 32);
            this.listIndexFiles.MultiSelect = false;
            this.listIndexFiles.Name = "listIndexFiles";
            this.listIndexFiles.Size = new System.Drawing.Size(736, 469);
            this.listIndexFiles.TabIndex = 2;
            this.listIndexFiles.UseCompatibleStateImageBehavior = false;
            this.listIndexFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnFilename
            // 
            this.columnFilename.Text = "Filename";
            this.columnFilename.Width = 300;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 100;
            // 
            // columnUnit
            // 
            this.columnUnit.Text = "";
            // 
            // lblFileSize
            // 
            this.lblFileSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblFileSize.Location = new System.Drawing.Point(96, 8);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(88, 13);
            this.lblFileSize.TabIndex = 1;
            this.lblFileSize.Text = "?";
            // 
            // labelIndexSize
            // 
            this.labelIndexSize.AutoSize = true;
            this.labelIndexSize.Location = new System.Drawing.Point(8, 8);
            this.labelIndexSize.Name = "labelIndexSize";
            this.labelIndexSize.Size = new System.Drawing.Size(86, 13);
            this.labelIndexSize.TabIndex = 0;
            this.labelIndexSize.Text = "Total Index Size:";
            // 
            // tabPlugins
            // 
            this.tabPlugins.Controls.Add(this.groupPlugin);
            this.tabPlugins.Controls.Add(this.lstPlugins);
            this.tabPlugins.ImageIndex = 5;
            this.tabPlugins.Location = new System.Drawing.Point(4, 23);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Size = new System.Drawing.Size(752, 509);
            this.tabPlugins.TabIndex = 4;
            this.tabPlugins.Text = "Plugins";
            // 
            // groupPlugin
            // 
            this.groupPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPlugin.Controls.Add(this.panelPlugin);
            this.groupPlugin.Controls.Add(this.groupPluginInfo);
            this.groupPlugin.Location = new System.Drawing.Point(163, 16);
            this.groupPlugin.Name = "groupPlugin";
            this.groupPlugin.Size = new System.Drawing.Size(581, 485);
            this.groupPlugin.TabIndex = 2;
            this.groupPlugin.TabStop = false;
            // 
            // panelPlugin
            // 
            this.panelPlugin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPlugin.Location = new System.Drawing.Point(8, 64);
            this.panelPlugin.Name = "panelPlugin";
            this.panelPlugin.Size = new System.Drawing.Size(565, 413);
            this.panelPlugin.TabIndex = 1;
            // 
            // groupPluginInfo
            // 
            this.groupPluginInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupPluginInfo.BackColor = System.Drawing.SystemColors.Control;
            this.groupPluginInfo.Controls.Add(this.linkPluginURL);
            this.groupPluginInfo.Controls.Add(this.lblPluginInfo);
            this.groupPluginInfo.Location = new System.Drawing.Point(8, 8);
            this.groupPluginInfo.Name = "groupPluginInfo";
            this.groupPluginInfo.Size = new System.Drawing.Size(565, 48);
            this.groupPluginInfo.TabIndex = 0;
            this.groupPluginInfo.TabStop = false;
            // 
            // linkPluginURL
            // 
            this.linkPluginURL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkPluginURL.Location = new System.Drawing.Point(389, 16);
            this.linkPluginURL.Name = "linkPluginURL";
            this.linkPluginURL.Size = new System.Drawing.Size(160, 23);
            this.linkPluginURL.TabIndex = 1;
            this.linkPluginURL.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkPluginURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPluginURL_LinkClicked);
            // 
            // lblPluginInfo
            // 
            this.lblPluginInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPluginInfo.Location = new System.Drawing.Point(8, 16);
            this.lblPluginInfo.Name = "lblPluginInfo";
            this.lblPluginInfo.Size = new System.Drawing.Size(373, 24);
            this.lblPluginInfo.TabIndex = 0;
            this.lblPluginInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstPlugins
            // 
            this.lstPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstPlugins.Location = new System.Drawing.Point(8, 16);
            this.lstPlugins.Name = "lstPlugins";
            this.lstPlugins.Size = new System.Drawing.Size(149, 485);
            this.lstPlugins.TabIndex = 1;
            this.lstPlugins.SelectedIndexChanged += new System.EventHandler(this.lstPlugins_SelectedIndexChanged);
            // 
            // Luke
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(760, 565);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(768, 592);
            this.Name = "Luke";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Luke";
            this.Load += new System.EventHandler(this.Luke_Load);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelMessage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanelLogo)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabOverview.ResumeLayout(false);
            this.tabOverview.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabDocuments.ResumeLayout(false);
            this.tabDocuments.PerformLayout();
            this.groupTerm.ResumeLayout(false);
            this.groupTerm.PerformLayout();
            this.groupDocNumber.ResumeLayout(false);
            this.groupDocNumber.PerformLayout();
            this.tabSearch.ResumeLayout(false);
            this.tabSearch.PerformLayout();
            this.groupSearchOptions.ResumeLayout(false);
            this.groupSearchOptions.PerformLayout();
            this.tabFiles.ResumeLayout(false);
            this.tabFiles.PerformLayout();
            this.tabPlugins.ResumeLayout(false);
            this.groupPlugin.ResumeLayout(false);
            this.groupPluginInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Luke());
        }


        #region Luke Events	
        private void Luke_Load(object sender, System.EventArgs e)
        {
            OpenIndex openIndexDlg = new OpenIndex(p);
            if (openIndexDlg.ShowDialog(this) == DialogResult.OK)
                LukeInit(openIndexDlg);
            else Close();
        }

        private void statusBar_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {
            if (e.StatusBarPanel == statusBarPanelLogo)
                System.Diagnostics.Process.Start(lukeURL);
        }

        private void domainTerms_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                domainTerms.SelectedIndex = Convert.ToInt32(domainTerms.Text);
            }
            catch (FormatException)
            { }
        }

        private void textTerm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar((int)Keys.Enter))
            {
                if (indexReader == null)
                {
                    ShowStatus(resources.GetString("NoIndex"));
                    return;
                }

                try
                {
                    String text = textTerm.Text;
                    String field = comboTerms.Text;

                    if (text == null || text == string.Empty)
                        return;

                    Term t = new Term(field, text);
                    _ShowTerm(t);
                }
                catch (Exception exc)
                {
                    ShowStatus(exc.Message);
                }
            }
        }

        private void textDocNum_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar((int)Keys.Enter))
                _ShowDoc(0);
        }

        private void listSearch_DoubleClick(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            if (listSearch.SelectedItems == null ||
                listSearch.SelectedItems.Count == 0)
                return;

            Document doc;
            int docId;
            try
            {
                docId = Int32.Parse(listSearch.SelectedItems[0].SubItems[1].Text);

                doc = indexReader.Document(docId);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
                return;
            }

            _ShowDocFields(docId, doc);

            tabControl.SelectedTab = tabDocuments;
        }

        private void listTerms_DoubleClick(object sender, System.EventArgs e)
        {
            contextMenuItemBrowse_Click(sender, e);
        }

        private void listTerms_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            listTerms.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listTerms.Sort();
        }
        private void listSearch_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            listSearch.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listSearch.Sort();
        }
        private void listDocFields_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            listDocFields.ListViewItemSorter = new ListViewItemComparer(e.Column);
            listDocFields.Sort();
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (tabControl.SelectedTab == tabOverview)
                SetOverviewContextMenuItems();
            else if (tabControl.SelectedTab == tabDocuments)
                SetDocumentsContextMenuItems();
        }
        #endregion Luke Events

        #region Resizing
        private void tabDocuments_Resize(object sender, System.EventArgs e)
        {
            ResizeLastListColumn(listDocFields);
        }

        private void tabOverview_Resize(object sender, System.EventArgs e)
        {
            ResizeLastListColumn(listTerms);
            ResizeLastListColumn(listFields);
        }

        private void ResizeLastListColumn(ListView list)
        {
            int width = list.Width;
            for (int i = 0; i < list.Columns.Count - 1; i++)
            {
                width -= list.Columns[i].Width;
            }
            list.Columns[list.Columns.Count - 1].Width = width - 22;

        }
        #endregion Resizing

        #region Buttons
        private void buttonPrevDoc_Click(object sender, System.EventArgs e)
        {
            ShowStatus("");
            _ShowDoc(-1);
        }

        private void buttonNextDoc_Click(object sender, System.EventArgs e)
        {
            ShowStatus("");
            _ShowDoc(+1);
        }

        private void buttonTopTerms_Click(object sender, System.EventArgs e)
        {
            ShowStatus("");
            ShowTopTerms();
        }

        private void buttonFirstTerm_Click(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            try
            {
                TermEnum te = indexReader.Terms();
                te.Next();
                Term t = te.Term;
                _ShowTerm(t);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void buttonNextTerm_Click(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            try
            {
                String text = textTerm.Text;
                String field = comboTerms.Text;

                TermEnum te;

                if (text == null || text == string.Empty)
                    te = indexReader.Terms();
                else
                    te = indexReader.Terms(new Term(field, text));

                te.Next();
                Term t = te.Term;

                _ShowTerm(t);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void buttonDelete_Click(object sender, System.EventArgs e)
        {
            int docid = 0;
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            if (readOnly)
            {
                ShowStatus(resources.GetString("Readonly"));
                return;
            }
            try
            {
                docid = Int32.Parse(textDocNum.Text);

                _ShowDoc(+1);
                indexReader.DeleteDocument(docid);
                //indexReader.Delete(docid);
                InitOverview();
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void buttonShowFirstDoc_Click(object sender, System.EventArgs e)
        {
            if (term == null) return;
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            try
            {
                termDocs = indexReader.TermDocs(term);
                termDocs.Next();
                labelDocNum.Text = "1";

                _ShowTermDoc(termDocs);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void buttonShowNextDoc_Click(object sender, System.EventArgs e)
        {
            if (term == null) return;

            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            try
            {
                if (termDocs == null)
                {
                    buttonShowFirstDoc_Click(sender, e);
                    return;
                }

                if (!termDocs.Next()) return;

                int cnt = 1;
                try
                {
                    cnt = Int32.Parse(labelDocNum.Text);
                }
                catch (Exception) { };

                labelDocNum.Text = (cnt + 1).ToString();

                _ShowTermDoc(termDocs);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void buttonShowAllDocs_Click(object sender, System.EventArgs e)
        {
            if (term == null) return;
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            tabControl.SelectedTab = tabSearch;

            textSearch.Text = term.Field + ":" + term.Text;

            Query q = new TermQuery(term);
            textParsed.Text = q.ToString();

            IndexSearcher searcher = null;
            try
            {
                searcher = new IndexSearcher(dir);

                listSearch.BeginUpdate();
                listSearch.Items.Clear();
                listSearch.EndUpdate();

                _Search(q, searcher);
            }
            catch (Exception exc)
            {
                ErrorMessage(exc.Message);
            }
            finally
            {
                if (searcher != null) try { searcher.Dispose(); }
                    catch (Exception) { };
            }
        }

        private void buttonDeleteAllDocs_Click(object sender, System.EventArgs e)
        {
            if (term == null) return;

            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            if (readOnly)
            {
                ShowStatus(resources.GetString("Readonly"));
                return;
            }
            try
            {
                buttonShowNextDoc_Click(sender, e);
                indexReader.DeleteDocuments(term);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
            finally
            {
                listDocFields.BeginUpdate();
                listDocFields.Items.Clear();
                listDocFields.EndUpdate();
            }
            InitOverview();
        }

        private void buttonSearch_Click(object sender, System.EventArgs e)
        {
            Search();
        }

        private void buttonSearchDelete_Click(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            if (readOnly)
            {
                ShowStatus(resources.GetString("Readonly"));
                return;
            }

            try
            {
                foreach (ListViewItem item in listSearch.SelectedItems)
                {
                    int docId = Int32.Parse(item.SubItems[1].Text);
                    indexReader.DeleteDocument(docId);
                    listSearch.Items.Remove(item);
                }
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
            InitOverview();
        }

        private void btnExplain_Click(object sender, System.EventArgs e)
        {
            if (listSearch.SelectedItems.Count == 0) return;
            if (searchedDocIds == null || searchedDocIds.Length < listSearch.Items.Count) return;

            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            if (query == null) return;

            IndexSearcher searcher = null;
            try
            {
                searcher = new IndexSearcher(dir);
                Lucene.Net.Search.Explanation expl = searcher.Explain(query, searchedDocIds[listSearch.SelectedIndices[0]]);
                ExplanationDialog explDialog = new ExplanationDialog(expl);
                explDialog.ShowDialog(this);
            }
            catch (Exception exc)
            {
                ErrorMessage(exc.Message);
            }
            finally
            {
                searcher.Dispose();
            }
        }
        private void btnUpdateParsedQuery_Click(object sender, System.EventArgs e)
        {
            QueryParser qp = CreateQueryParser();
            String queryS = textSearch.Text;

            if (queryS.Trim() == "")
            {
                textParsed.Enabled = false;
                textParsed.Text = resources.GetString("EmptyQuery");
                return;
            }
            textParsed.Enabled = true;

            try
            {
                Query q = qp.Parse(queryS);
                textParsed.Text = q.ToString();
            }
            catch (Exception exc)
            {
                textParsed.Text = exc.Message;
            }
        }
        private void btnTermVector_Click(object sender, System.EventArgs e)
        {
            ShowTV();
        }

        #endregion Buttons

        #region Clipboard
        private void buttonCopySelected_Click(object sender, System.EventArgs e)
        {
            if (listDocFields.SelectedItems == null ||
                listDocFields.SelectedItems.Count == 0)
                return;

            if (document == null) return;

            StringBuilder copyText = new StringBuilder();

            bool store = false,
                index = false,
                token = false,
                tvf = false;
            float boost;

            int i = 0;
            foreach (ListViewItem item in listDocFields.SelectedItems)
            {
                boost = 0;
                if (i++ > 0) copyText.Append(Environment.NewLine);

                if (item.SubItems[1].Text == "+")
                    index = true;
                else
                    index = false;
                if (item.SubItems[2].Text == "+")
                    token = true;
                else
                    token = false;
                if (item.SubItems[3].Text == "+")
                    store = true;
                else
                    store = false;
                if (item.SubItems[4].Text == "+")
                    tvf = true;
                else
                    tvf = false;
                try
                {
                    boost = Single.Parse(item.SubItems[5].Text, NumberFormatInfo.InvariantInfo);
                }
                catch (Exception) { }

                Field field = new Field(item.SubItems[0].Text.Trim().Substring(1, item.SubItems[0].Text.Trim().Length - 2),
                    item.SubItems[item.SubItems.Count - 1].Text,
                    store ? Field.Store.YES : Field.Store.NO,
                    index ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED,
                    //token?,
                    tvf ? Field.TermVector.YES : Field.TermVector.NO);
                field.Boost = boost;

                copyText.Append(field.ToString());

            }

            Clipboard.SetDataObject(copyText.ToString());
        }

        private void buttonCopyAll_Click(object sender, System.EventArgs e)
        {
            if (document == null) return;

            StringBuilder copyText = new StringBuilder();
            int i = 0;
            bool store = false,
                index = false,
                token = false,
                tvf = false;
            float boost;

            foreach (ListViewItem item in listDocFields.Items)
            {
                boost = 0;

                if (item.SubItems[1].Text == "+")
                    index = true;
                else
                    index = false;
                if (item.SubItems[2].Text == "+")
                    token = true;
                else
                    token = false;
                if (item.SubItems[3].Text == "+")
                    store = true;
                else
                    store = false;
                if (item.SubItems[4].Text == "+")
                    tvf = true;
                else
                    tvf = false;
                try
                {
                    boost = Single.Parse(item.SubItems[5].Text, NumberFormatInfo.InvariantInfo);
                }
                catch (Exception) { }

                if (!index && !token && !store && !tvf &&
                    item.SubItems[item.SubItems.Count - 1].Text == resources.GetString("FieldNotAvailable"))
                    continue;

                if (i++ > 0) copyText.Append(Environment.NewLine);

                Field field = new Field(item.SubItems[0].Text.Trim().Substring(1, item.SubItems[0].Text.Trim().Length - 2),
                    item.SubItems[item.SubItems.Count - 1].Text,
                    store ? Field.Store.YES : Field.Store.NO,
                    index ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED,
                    //token,
                    tvf ? Field.TermVector.YES : Field.TermVector.NO);
                field.Boost = boost;

                copyText.Append(field.ToString());

            }

            Clipboard.SetDataObject(copyText.ToString());
        }

        #endregion Clipboard

        #region Menu Events
        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void menuItemOptimize_Click(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            if (readOnly)
            {
                ShowStatus(resources.GetString("Readonly"));
                return;
            }

            try
            {
                indexReader.Dispose();
                IndexWriter writer = new IndexWriter(dir,
                    new WhitespaceAnalyzer(),
                    false, IndexWriter.MaxFieldLength.UNLIMITED);
                writer.UseCompoundFile = useCompound;
                long startSize = CalcTotalFileSize(dir);
                DateTime startTime = DateTime.Now;
                writer.Optimize();
                DateTime endTime = DateTime.Now;
                long endSize = CalcTotalFileSize(dir);
                long deltaSize = startSize - endSize;
                String sign = deltaSize < 0 ? " Increased " : " Reduced ";
                String sizeMsg = sign + NormalizeSize(Math.Abs(deltaSize)) + NormalizeUnit(Math.Abs(deltaSize));
                String timeMsg = ((TimeSpan)(endTime - startTime)).TotalMilliseconds + " ms";
                ShowStatus(sizeMsg + " in " + timeMsg);
                ShowFiles(dir);
                writer.Dispose();
                indexReader = IndexReader.Open(dir, true);

                InitOverview();
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }

        private void menuItemAbout_Click(object sender, System.EventArgs e)
        {
            About a = new About();
            a.ShowDialog(this);
        }

        private void OnOpenIndex(object sender, System.EventArgs e)
        {
            OpenIndex openIndexDlg = new OpenIndex(p);
            if (openIndexDlg.ShowDialog(this) == DialogResult.OK)
                LukeInit(openIndexDlg);
        }

        private void contextMenuItemShowAll_Click(object sender, System.EventArgs e)
        {
            if (listTerms.SelectedItems == null) return;

            ListViewItem selItem = listTerms.SelectedItems[0];
            if (selItem == null) return;

            string field = selItem.SubItems[2].Text.Trim().Substring(1, selItem.SubItems[2].Text.Trim().Length - 2);
            string text = selItem.SubItems[3].Text;

            if (field == null || text == null)
                return;

            Term t = new Term(field, text);

            tabControl.SelectedTab = tabSearch;

            textSearch.Text = t.Field + ":" + t.Text;
            Search();
        }

        private void contextMenuItemBrowse_Click(object sender, System.EventArgs e)
        {
            if (listTerms.SelectedItems == null) return;

            ListViewItem selItem = listTerms.SelectedItems[0];
            if (selItem == null) return;

            string field = selItem.SubItems[2].Text.Trim().Substring(1, selItem.SubItems[2].Text.Trim().Length - 2);
            string text = selItem.SubItems[3].Text;

            if (field == null || text == null)
                return;

            Term t = new Term(field, text);

            tabControl.SelectedTab = tabDocuments;
            _ShowTerm(t);
        }

        private void contextMenuItemShowTV_Click(object sender, System.EventArgs e)
        {
            ShowTV();
        }

        private void menuItemCompound_Click(object sender, System.EventArgs e)
        {
            menuItemCompound.Checked = !menuItemCompound.Checked;
            useCompound = menuItemCompound.Checked;
            p.UseCompound = useCompound;
        }

        private void menuItemUndelete_Click(object sender, System.EventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            if (readOnly)
            {
                ShowStatus(resources.GetString("Readonly"));
                return;
            }
            try
            {
                indexReader.UndeleteAll();
                InitOverview();
            }
            catch (Exception exc)
            {
                ErrorMessage(exc.Message);
            }
        }

        #endregion Menu Events

        #region Properties
        public IndexReader IndexReader
        {
            get
            { return indexReader; }
            set
            { indexReader = value; }
        }

        private string IndexName
        {
            set
            {
                labelName.Text = value;
                statusBar.Panels[0].Text = resources.GetString("StatusIndexName") + value;
            }
        }
        private string IndexVersion
        {
            set
            {
                labelVersion.Text = value;
            }
        }
        private string HasDeletions
        {
            set
            {
                labelDeletions.Text = value;
            }
        }

        private int DocumentsNumber
        {
            set
            {
                labelDocs.Text = value.ToString();
                labelIndDocs.Text = (value - 1).ToString();
            }
        }

        private int TermsNumber
        {
            set
            {
                numTerms = value;
                labelTerms.Text = value.ToString();
            }
        }

        private long LastModified
        {
            set
            {
                labelMod.Text = new DateTime(value * TimeSpan.TicksPerMillisecond).AddYears(1969).ToLocalTime().ToString();
            }
        }
        #endregion Properties

        #region Private Methods
        private void ShowTV()
        {
            if (listDocFields.SelectedItems.Count == 0) return;
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            int docId;
            try
            {
                docId = Int32.Parse(textDocNum.Text);
            }
            catch (Exception)
            {
                ShowStatus(resources.GetString("DocNotSelected"));
                return;
            }

            try
            {

                string fieldName = listDocFields.SelectedItems[0].SubItems[0].Text;
                fieldName = fieldName.Substring(1, fieldName.Length - 2);
                ITermFreqVector tfv = indexReader.GetTermFreqVector(docId, fieldName);
                if (tfv == null)
                {
                    ShowStatus(resources.GetString("NoTV"));
                    return;
                }

                TermVector tvDialog = new TermVector(fieldName, tfv);
                tvDialog.ShowDialog(this);
            }
            catch (Exception exc)
            {
                ShowStatus(exc.Message);
            }
        }
        internal void PopulateAnalyzers(ComboBox cbAnalyzers)
        {
            cbAnalyzers.BeginUpdate();
            cbAnalyzers.Items.Clear();

            string[] aNames = new String[analyzers.Count];
            analyzers.Keys.CopyTo(aNames, 0);
            cbAnalyzers.Items.AddRange(aNames);
            cbAnalyzers.EndUpdate();
            cbAnalyzers.SelectedIndex = 0;
        }

        public IDictionary GetAnalyzers()
        {
            return analyzers;
        }

        internal void InitOverview()
        {
            // populate analyzers
            PopulateAnalyzers(comboAnalyzer);

            IndexName = indexPath + (readOnly ? " (R)" : "");

            DocumentsNumber = indexReader.NumDocs();

            SetFieldNames(indexReader.GetFieldNames(IndexReader.FieldOption.ALL));

            TermEnum termsEnum = indexReader.Terms();
            int i = 0;
            while (termsEnum.Next()) i++;
            termsEnum.Dispose();

            TermsNumber = i;
            IndexVersion = IndexReader.GetCurrentVersion(dir).ToString();
            HasDeletions = indexReader.HasDeletions.ToString();

            LastModified = IndexReader.LastModified(dir);

            ShowTopTerms();
        }
        private void SetOverviewContextMenuItems()
        {
            contextMenu.MenuItems.Clear();

            contextMenuItemBrowse = new MenuItem(resources.GetString("MenuBrowse"));
            contextMenuItemBrowse.Click += new EventHandler(contextMenuItemBrowse_Click);
            contextMenu.MenuItems.Add(contextMenuItemBrowse);

            contextMenuItemShowAll = new MenuItem(resources.GetString("MenuShowAll"));
            contextMenuItemShowAll.Click += new EventHandler(contextMenuItemShowAll_Click);
            contextMenu.MenuItems.Add(contextMenuItemShowAll);
        }

        private void SetDocumentsContextMenuItems()
        {
            contextMenu.MenuItems.Clear();

            contextMenuItemShowTV = new MenuItem(resources.GetString("MenuShowTV"));
            contextMenuItemShowTV.Click += new EventHandler(contextMenuItemShowTV_Click);
            contextMenu.MenuItems.Add(contextMenuItemShowTV);
        }

        private void LukeInit(OpenIndex openIndexDlg)
        {
            indexPath = openIndexDlg.Path;
            readOnly = openIndexDlg.ReadOnlyIndex;
            bool force = openIndexDlg.UnlockIndex;

            if (dir != null)
            {
                try
                {
                    if (indexReader != null)
                        indexReader.Dispose();
                }
                catch (Exception) { };
                try
                {
                    dir.Dispose();
                }
                catch (Exception) { };
            }

            dir = new SimpleFSDirectory(new DirectoryInfo(indexPath));

            if (indexPath == String.Empty ||
                !IndexReader.IndexExists(dir))
            {
                ErrorMessage(resources.GetString("InvalidPath"));
                return;
            }

            try
            {
                p.AddToMruList(indexPath);

                Text = $"Luke - {indexPath}";
                if (IndexWriter.IsLocked(dir))
                {
                    if (readOnly)
                    {
                        ShowStatus(resources.GetString("IndexLockedRo"));
                        dir.Dispose();
                        return;
                    }
                    if (force)
                    {
                        IndexWriter.Unlock(dir);
                    }
                    else
                    {
                        ShowStatus(resources.GetString("IndexLocked"));
                        dir.Dispose();
                        return;
                    }
                }

                // files tab
                ShowFiles(dir);

                indexReader = IndexReader.Open(dir, true);

                menuItemCompound.Checked = p.UseCompound;

                // load analyzers
                try
                {

                    Type[] analyzerTypes = null;
                    try
                    {
                        analyzerTypes = Lucene.Net.LukeNet.ClassFinder.ClassFinder.GetInstantiableSubtypes(typeof(Lucene.Net.Analysis.Analyzer));
                    }
                    catch (Exception) { }

                    if (analyzerTypes == null || analyzerTypes.Length == 0)
                    {
                        // using default
                        foreach (Type t in defaultAnalyzers)
                            analyzers[t.FullName] = t;
                    }
                    else
                    {
                        foreach (Type aType in analyzerTypes)
                            analyzers[aType.FullName] = aType;
                    }
                }
                catch (Exception e)
                {
                    ErrorMessage(e.Message);
                }

                // plugins tab
                LoadPlugins();
                InitPlugins();

                // overview tab
                InitOverview();

                // search tab
                // Remove columns
                listSearch.BeginUpdate();

                while (listSearch.Columns.Count > 2)
                {
                    listSearch.Columns.RemoveAt(2);
                }

                foreach (string field in indexFields)
                {
                    listSearch.Columns.Add(field, 200, HorizontalAlignment.Left);
                }
                listSearch.EndUpdate();

                tabOverview_Resize(null, null);
                tabDocuments_Resize(null, null);

                ShowStatus(resources.GetString("IndexOpened"));
            }
            catch (Exception e)
            {
                ErrorMessage(e.Message);
            }
        }

        internal void ErrorMessage(string msg)
        {
            MessageBox.Show(this,
                            msg,
                            resources.GetString("Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        delegate void ShowStatusDelegate(string message);
        /// <summary>
        /// Updates status. Possibly async
        /// </summary>
        internal void ShowStatus(string message)
        {
            if (!statusBar.InvokeRequired)
                statusBar.Panels[1].Text = message;
            else
            {
                // Show status asynchronously
                ShowStatusDelegate showStatus = new ShowStatusDelegate(ShowStatus);
                BeginInvoke(showStatus, new object[] { message });
            }
        }

        private void SetFieldNames(ICollection<string> names)
        {
            indexFields = new String[names.Count];
            labelFields.Text = names.Count.ToString();

            int i = 0;

            listFields.BeginUpdate();
            listFields.Items.Clear();
            comboFields.Items.Clear();
            comboTerms.Items.Clear();

            foreach (var name in names)
            {
                // skip empty field
                listFields.Items.Add(new ListViewItem("<" + name + ">"));
                comboFields.Items.Add(name);
                comboTerms.Items.Add(name);
                indexFields[i++] = name;
            }

            listFields.EndUpdate();
            comboFields.SelectedIndex = 0;
            comboTerms.SelectedIndex = 0;
        }

        private void Search(bool isParse = true)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            queryParser = CreateQueryParser();


            string queryString = textSearch.Text;
            if (queryString == string.Empty)
            {
                ShowStatus(resources.GetString("EmptyQuery"));
                return;
            }

            IndexSearcher searcher = null;

            listSearch.BeginUpdate();
            listSearch.Items.Clear();

            listSearch.EndUpdate();
            try
            {
                Query q = queryParser.Parse(queryString);

                if (searcher != null) searcher.Dispose();
                searcher = new IndexSearcher(dir);

                textParsed.Text = q.ToString();
                _Search(q, searcher);
            }
            catch (Exception e)
            {
                ErrorMessage(e.Message);
            }
            finally
            {
                if (searcher != null) try { searcher.Dispose(); }
                    catch (Exception) { };
            }

            Cursor.Current = Cursors.Default;
        }

        private void _Search(Query q, IndexSearcher searcher)
        {
            DateTime start = DateTime.Now;
            TopDocs hits = searcher.Search(q, 99999);
            ShowStatus(((TimeSpan)(DateTime.Now - start)).TotalMilliseconds.ToString() + " ms");

            listSearch.BeginUpdate();

            try
            {
                if (hits == null || hits.TotalHits == 0)
                {
                    if (listSearch.Columns.Count < 3)
                    {
                        int width = listSearch.Width -
                            listSearch.Columns[0].Width -
                            listSearch.Columns[1].Width;
                        listSearch.Columns.Add("", width, HorizontalAlignment.Left);
                    }

                    ListViewItem noResults = new ListViewItem();
                    noResults.SubItems.AddRange(new String[] { "", resources.GetString("NoResults") });
                    listSearch.Items.Add(noResults);

                    labelSearchRes.Text = "0";
                    return;
                }

                labelSearchRes.Text = hits.TotalHits.ToString();

                searchedDocIds = new int[hits.TotalHits];

                for (int i = 0; i < hits.TotalHits; i++)
                {
                    ListViewItem item = new ListViewItem((Math.Round((double)1000 * hits.ScoreDocs[i].Score, 1) / 10).ToString());
                    item.SubItems.Add(hits.ScoreDocs[i].Doc.ToString());

                    Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);
                    searchedDocIds[i] = hits.ScoreDocs[i].Doc;

                    for (int j = 0; j < indexFields.Length; j++)
                    {
                        item.SubItems.Add(doc.Get(indexFields[j]));
                    }

                    listSearch.Items.Add(item);
                }
                query = q;
            }
            finally
            {
                listSearch.EndUpdate();
            }
        }

        private void ShowTopTerms()
        {
            int nDoc = (int)domainTerms.SelectedItem;
            ListView.CheckedListViewItemCollection fields = listFields.CheckedItems;

            String[] selectedFields;
            if (fields == null || fields.Count == 0)
            {
                selectedFields = indexFields;
            }
            else
            {
                selectedFields = new String[fields.Count];
                int i = 0;
                foreach (ListViewItem item in fields)
                {
                    // item.Text without "<", ">"
                    selectedFields[i++] = item.Text.Substring(1, item.Text.Length - 2);
                }
            }

            try
            {
                TermInfo[] termInfos = HighFreqTerms.GetHighFreqTerms(dir, null, nDoc, selectedFields);

                listTerms.BeginUpdate();
                listTerms.Items.Clear();

                if (termInfos == null || termInfos.Length == 0)
                {
                    ListViewItem noResults = new ListViewItem();
                    noResults.SubItems.AddRange(new String[] { "", "", resources.GetString("NoResults") });
                    listTerms.Items.Add(noResults);
                    return;
                }

                for (int i = 0; i < termInfos.Length; i++)
                {
                    ListViewItem item = new ListViewItem((i + 1).ToString());
                    item.SubItems.AddRange(
                        new String[]{
                            termInfos[i].DocFreq.ToString(),
                            " <" + termInfos[i].Term.Field + "> ",
                            termInfos[i].Term.Text});

                    listTerms.Items.Add(item);
                }
            }
            catch (Exception e)
            {
                ErrorMessage(e.Message);
            }
            finally
            {
                listTerms.EndUpdate();
            }
        }

        private Document CreateDocument(int iNum)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return null;
            }

            if (iNum < 0 || iNum >= indexReader.NumDocs())
            {
                ShowStatus(resources.GetString("DocNumberRange"));
                return null;
            }

            if (!indexReader.IsDeleted(iNum))
                return indexReader.Document(iNum);
            else
            {
                ShowStatus(resources.GetString("DocDeleted"));
                return null;
            }
        }
        private void _ShowDoc(int incr)
        {
            string num = textDocNum.Text;

            if (num == string.Empty) num = (-incr).ToString();

            try
            {
                int iNum = Int32.Parse(num);
                iNum += incr;
                Document doc = CreateDocument(iNum);

                _ShowDocFields(iNum, doc);
            }
            catch (Exception e)
            {
                ShowStatus(e.Message);
            }
        }
        private void _ShowDocFields(int docid, Document doc)
        {
            textDocNum.Text = docid.ToString();
            labelInfoDocNum.Text = docid.ToString();

            listDocFields.BeginUpdate();
            listDocFields.Items.Clear();

            document = doc;

            try
            {
                if (doc == null) return;
                for (int i = 0; i < indexFields.Length; i++)
                {
                    Field[] fields = doc.GetFields(indexFields[i]);
                    if (fields == null)
                    {
                        AddFieldRow(indexFields[i], null);
                        continue;
                    }
                    for (int j = 0; j < fields.Length; j++)
                    {
                        AddFieldRow(indexFields[i], fields[j]);
                    }
                }
            }
            finally
            {
                listDocFields.EndUpdate();
            }
        }

        private void AddFieldRow(string fieldName, Field f)
        {
            string feature;
            ListViewItem item = new ListViewItem("<" + fieldName + ">");

            if (f != null && f.IsIndexed) feature = "+";
            else feature = " ";
            item.SubItems.Add(feature);

            if (f != null && f.IsTokenized) feature = "+";
            else feature = " ";
            item.SubItems.Add(feature);

            if (f != null && f.IsStored) feature = "+";
            else feature = " ";
            item.SubItems.Add(feature);

            if (f != null && f.IsTermVectorStored) feature = "+";
            else feature = " ";
            item.SubItems.Add(feature);

            if (f != null)
                item.SubItems.Add(Lucene.Net.Util.Number.ToString(f.Boost));
            else item.SubItems.Add("");

            if (f != null)
                item.SubItems.Add(f.StringValue);
            else
            {
                item.SubItems.Add(resources.GetString("FieldNotAvailable"));
            }

            listDocFields.Items.Add(item);
        }

        private void _ShowTerm(Term t)
        {
            if (t == null)
            {
                ShowStatus(resources.GetString("NoTerms"));
                return;
            }
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            termDocs = null;
            this.term = t;
            comboTerms.SelectedItem = t.Field;
            textTerm.Text = t.Text;

            labelDocNum.Text = "?";
            labelTermFreq.Text = "?";

            try
            {
                int freq = indexReader.DocFreq(t);
                labelDocFreq.Text = freq.ToString();
                labelDocMax.Text = freq.ToString();
            }
            catch (Exception e)
            {
                ShowStatus(e.Message);
                labelDocFreq.Text = "?";
            }
        }

        private void _ShowTermDoc(TermDocs td)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }
            try
            {
                Document doc = indexReader.Document(td.Doc);

                labelDocNum.Text = td.Doc.ToString();
                labelTermFreq.Text = td.Freq.ToString();

                _ShowDocFields(td.Doc, doc);
            }
            catch (Exception e)
            {
                ShowStatus(e.Message);
            }
        }
        public Analyzer AnalyzerForName(string analyzerName)
        {
            try
            {
                // Trying to create type from executing assembly
                Type analyzerType = (Type)analyzers[analyzerName];

                if (null == analyzerType)
                {
                    // Trying to create type from Lucene.Net assembly
                    Assembly a = Assembly.GetAssembly(typeof(Lucene.Net.Analysis.Analyzer));
                    analyzerType = a.GetType(analyzerName);
                }

                // Trying to create with default constructor
                return (Analyzer)Activator.CreateInstance(analyzerType, Util.Version.LUCENE_30);
            }
            catch (Exception)
            { return null; }
        }

        private QueryParser CreateQueryParser()
        {
            string analyzerName = (string)comboAnalyzer.SelectedItem;
            if (null == analyzerName || analyzerName == string.Empty)
            {
                analyzerName = "Lucene.Net.Luca.Analysis.Standard.StandardAnalyzer";
                comboAnalyzer.SelectedItem = analyzerName;
            }

            analyzer = AnalyzerForName(analyzerName);
            if (null == analyzer)
            {
                ErrorMessage(string.Format(resources.GetString("AnalyzerNotFound"), analyzerName));
                analyzer = stdAnalyzer;
            }

            string defField = (string)comboFields.SelectedItem;
            if (defField == null || defField == string.Empty)
            {
                defField = indexFields[0];
                comboFields.SelectedItem = analyzerName;
            }

            return new QueryParser(Util.Version.LUCENE_30, defField, analyzer);
        }
        #endregion Private Methods

        #region Plugins
        private void LoadPlugins()
        {
            Hashtable pluginTypes = new Hashtable();
            // try to find all plugins
            try
            {
                Type[] types =
                    Lucene.Net.LukeNet.ClassFinder.ClassFinder.GetInstantiableSubtypes(typeof(Lucene.Net.LukeNet.Plugins.LukePlugin));
                foreach (Type type in types)
                {
                    pluginTypes.Add(type, null);
                }
            }
            catch (Exception) { }

            // load plugins declared in the ".plugins" file
            //try 
            //{
            //    StreamReader reader = new StreamReader("plugins.luke");
            //    string line;
            //    while ((line = reader.ReadLine()) != null) 
            //    {
            //        if (line.StartsWith("#")) continue;
            //        if (line.Trim().Equals("")) continue;

            //        var t = Type.GetType(line, false);
            //        if (t.IsSubclassOf(typeof(LukePlugin)) && !pluginTypes.Contains(t)) 
            //        {
            //            pluginTypes.Add(t, null);
            //        }
            //    }
            //} 
            //catch (IOException) 
            //{}

            foreach (Type pluginType in pluginTypes.Keys)
            {
                try
                {
                    LukePlugin plugin = (LukePlugin)pluginType.GetConstructor(new Type[] { }).Invoke(new Object[] { });
                    plugins.Add(plugin);
                }
                catch (Exception e)
                {
                    ShowStatus(string.Format("{0} PLUGIN ERROR: {1}", pluginType.Name, e.Message));
                }
            }
            if (plugins.Count == 0) return;
            InitPlugins();
        }

        internal void InitPlugins()
        {
            ClearePluginsUI();

            foreach (LukePlugin plugin in plugins)
            {
                plugin.SetDirectory(dir);
                plugin.SetIndexReader(indexReader);
                try
                {
                    plugin.Init();
                    plugin.Anchor = AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                }
                catch (Exception e)
                {
                    plugins.Remove(plugin);
                    ShowStatus("PLUGIN ERROR: " + e.Message);
                }
                lstPlugins.Items.Add(plugin.GetPluginName());
            }

            if (lstPlugins.Items.Count > 0)
            {
                lstPlugins.SelectedIndex = 0;
            }
        }

        private void ClearePluginsUI()
        {
            lstPlugins.Items.Clear();
            panelPlugin.Controls.Clear();
            lblPluginInfo.Text = "";
            linkPluginURL.Text = "";
            linkPluginURL.Links.Clear();

        }

        public void LoadPluginTab(LukePlugin plugin)
        {
            lblPluginInfo.Text = plugin.GetPluginInfo();
            linkPluginURL.Text = plugin.GetPluginHome();
            linkPluginURL.Links.Clear();
            linkPluginURL.Links.Add(0, linkPluginURL.Text.Length, linkPluginURL.Text);

            plugin.Size = panelPlugin.Size;
            panelPlugin.Controls.Clear();
            panelPlugin.Controls.Add(plugin);
        }
        private void lstPlugins_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            LoadPluginTab((LukePlugin)plugins[lstPlugins.SelectedIndex]);
        }

        private void linkPluginURL_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            linkPluginURL.Links[linkPluginURL.Links.IndexOf(e.Link)].Visited = true;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
        #endregion Plugins

        #region Files
        public void ShowFiles(FSDirectory dir)
        {
            String[] files = dir.ListAll();

            listIndexFiles.Items.Clear();

            foreach (string file in files)
            {
                //				FileInfo fi = new FileInfo(file);

                ListViewItem row = new ListViewItem(
                    new string[]
                    {
                        file, //fi.Name,
						NormalizeSize(dir.FileLength(file)),
                        NormalizeUnit(dir.FileLength(file))
                    });
                listIndexFiles.Items.Add(row);
            }

            long totalFileSize = CalcTotalFileSize(dir);
            lblFileSize.Text = NormalizeSize(totalFileSize) + NormalizeUnit(totalFileSize);
        }

        private long CalcTotalFileSize(FSDirectory dir)
        {
            long totalFileSize = 0;
            foreach (string file in dir.ListAll())
            {
                totalFileSize += dir.FileLength(file);
                //				FileInfo fi = new FileInfo(file);
                //				totalFileSize += fi.Length;
            }

            return totalFileSize;
        }

        private String NormalizeUnit(long len)
        {
            if (len == 1)
            {
                return "Byte";
            }
            else if (len < 1024)
            {
                return "Bytes";
            }
            else if (len < 51200000)
            {
                return "Kb";
            }
            else
            {
                return "Mb";
            }
        }

        private String NormalizeSize(long len)
        {
            if (len < 1024)
            {
                return len.ToString();
            }
            else if (len < 51200000)
            {
                return ((long)(len / 1024)).ToString();
            }
            else
            {
                return ((long)(len / 102400)).ToString();
            }
        }
        #endregion Files

        #region Document reconstruction
        private void btnReconstruct_Click(object sender, System.EventArgs e)
        {
            progressDlg = new Progress();
            progressDlg.Message = resources.GetString("CollectingTerms");

            int docNum = 0;
            try
            {
                docNum = Int32.Parse(textDocNum.Text);
            }
            catch (Exception)
            {
                ShowStatus(resources.GetString("DocNotSelected"));
                return;
            }

            Document document = CreateDocument(docNum);
            if (document == null)
                return;

            Hashtable doc = new Hashtable();

            this.Cursor = Cursors.WaitCursor;

            // async call to reconstruction
            ReconstructDelegate reconstruct = new ReconstructDelegate(BeginAsyncReconstruction);
            reconstruct.BeginInvoke(docNum, document, doc, new AsyncCallback(EndAsyncReconstruction), null);

            progressDlg.ShowDialog(this);
            this.Cursor = Cursors.Default;

            EditDocument editDocDlg = new EditDocument(this, docNum, document, doc);
            editDocDlg.ShowDialog();
            if (editDocDlg.DialogResult == DialogResult.OK)
            {
                InitOverview();
                InitPlugins();
            }
        }

        void EndAsyncReconstruction(IAsyncResult target)
        {
            if (!progressDlg.InvokeRequired)
                progressDlg.Close();
            else
            {
                // Show status asynchronously
                AsyncCallback endReconstruct = new AsyncCallback(EndAsyncReconstruction);
                BeginInvoke(endReconstruct, new object[] { target });
            }
        }
        delegate void ReconstructDelegate(int docNum, Document document, Hashtable doc);
        void BeginAsyncReconstruction(int docNum, Document document, Hashtable doc)
        {
            // get stored fields
            ArrayList sf = new ArrayList();
            for (int i = 0; i < indexFields.Length; i++)
            {
                Field[] f = document.GetFields(indexFields[i]);
                if (f == null || !f[0].IsStored) continue;
                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < f.Length; k++)
                {
                    if (k > 0) sb.Append('\n');
                    sb.Append(f[k].StringValue);
                }
                Field field = new Field(indexFields[i], sb.ToString(),
                    f[0].IsStored ? Field.Store.YES : Field.Store.NO,
                    f[0].IsIndexed ? Field.Index.ANALYZED : Field.Index.NOT_ANALYZED,
                    //f[0].IsTokenized()?,
                    f[0].IsTermVectorStored ? Field.TermVector.YES : Field.TermVector.NO);
                field.Boost = f[0].Boost;
                doc[indexFields[i]] = field;
                sf.Add(indexFields[i]);
            }
            String term = null;
            GrowableStringArray terms = null;
            try
            {
                int i = 0;
                int delta = (int)Math.Ceiling(((double)numTerms / 100));

                TermEnum te = indexReader.Terms();
                TermPositions tp = indexReader.TermPositions();
                while (te.Next())
                {
                    if ((i++ % delta) == 0)
                    {
                        // update UI - async
                        UpdateProgress(i / delta);
                    }

                    // skip stored fields
                    if (sf.Contains(te.Term.Field)) continue;
                    tp.Seek(te.Term);
                    if (!tp.SkipTo(docNum) || tp.Doc != docNum)
                    {
                        // this term is not found in the doc
                        continue;
                    }
                    term = te.Term.Text;
                    terms = (GrowableStringArray)doc[te.Term.Field];
                    if (terms == null)
                    {
                        terms = new GrowableStringArray();
                        doc[te.Term.Field] = terms;
                    }
                    for (int k = 0; k < tp.Freq; k++)
                    {
                        int pos = tp.NextPosition();
                        terms.Set(pos, term);
                    }
                }
            }
            catch (Exception exc)
            {
                // Update UI - async
                ShowStatus(exc.Message);
            }
        }

        delegate void UpdateProgressDelegate(int val);
        /// <summary>
        /// Updates status. Possibly async
        /// </summary>
        private void UpdateProgress(int val)
        {
            if (!progressDlg.InvokeRequired)
                progressDlg.Value = val;
            else
            {
                // Show status asynchronously
                UpdateProgressDelegate showProgress = new UpdateProgressDelegate(UpdateProgress);
                BeginInvoke(showProgress, new object[] { val });
            }
        }
        #endregion Document reconstruction

        #region class ListViewItemComparer
        internal class ListViewItemComparer : IComparer
        {
            private int col;
            public ListViewItemComparer()
            {
                col = 0;
            }
            public ListViewItemComparer(int column)
            {
                col = column;
            }
            public int Compare(object x, object y)
            {
                ListViewItem itemX = x as ListViewItem;
                ListViewItem itemY = y as ListViewItem;

                Debug.Assert(itemX != null && itemY != null);

                // First two columns - numbers
                if (col == 0 || col == 1)
                {
                    try
                    {
                        return Int32.Parse(itemX.SubItems[col].Text) -
                               Int32.Parse(itemY.SubItems[col].Text);
                    }
                    catch (Exception)
                    { }
                }

                return String.Compare(itemX.SubItems[col].Text,
                                      itemY.SubItems[col].Text);
            }
        }
        #endregion class ListViewItemComparer

        private void textParsed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (indexReader == null)
            {
                ShowStatus(resources.GetString("NoIndex"));
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            queryParser = CreateQueryParser();


            string queryString = textSearch.Text;
            if (queryString == string.Empty)
            {
                ShowStatus(resources.GetString("EmptyQuery"));
                return;
            }

            IndexSearcher searcher = null;

            listSearch.BeginUpdate();
            listSearch.Items.Clear();

            listSearch.EndUpdate();
            try
            {
                Query q = queryParser.Parse(queryString);

                if (searcher != null) searcher.Dispose();
                searcher = new IndexSearcher(dir);

                textParsed.Text = q.ToString();
                _Search(q, searcher);
            }
            catch (Exception exc)
            {
                ErrorMessage(exc.Message);
            }
            finally
            {
                if (searcher != null) try { searcher.Dispose(); }
                    catch (Exception) { };
            }

            Cursor.Current = Cursors.Default;
        }

        private void textSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Enter))
            {
                e.SuppressKeyPress = true;
                buttonSearch_Click(null, null);
            }
        }
    }

    #region class GrowableStringArray
    class GrowableStringArray
    {
        public int INITIAL_SIZE = 20;
        private int size = 0;
        private String[] array = null;

        public int Size()
        {
            return size;
        }

        public void Set(int index, String val)
        {
            if (array == null) array = new String[INITIAL_SIZE];
            if (array.Length < index + 1)
            {
                String[] newArray = new String[index + INITIAL_SIZE];
                array.CopyTo(newArray, 0);
                array = newArray;
            }
            if (index > size - 1) size = index + 1;
            array[index] = val;
        }

        public String Get(int index)
        {
            return array[index];
        }
    }
    #endregion class GrowableStringArray
}

