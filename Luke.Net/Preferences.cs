using System;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections;

namespace Lucene.Net.LukeNet
{
	/// <author>
	/// abial
	/// </author>
	public class Preferences
	{
		[NonSerialized]
		public const string LUKE_PREFS_FILE      = ".luke";
		private static readonly string HOME_DIR;

		public string LastPwd
		{
			get { return pd.LastPwd;}
			set { pd.LastPwd = value;}
		}
		public ArrayList MruList
		{
			get { return pd.MruList;}
			set { pd.MruList = value;}
		}
		public int MruMaxSize
		{
			get { return pd.MruMaxSize;}
			set { pd.MruMaxSize = value;}
		}
		public bool UseCompound
		{
			get { return pd.UseCompound;}
			set { pd.UseCompound = value;}
		}

		private static string prefsFile;

		private PrefsData pd;
		public Preferences()
		{
			pd = new PrefsData();
			UseCompound = false;
			MruList = new ArrayList();
			MruMaxSize = 10;
		}

		static Preferences()
		{
			FileInfo file = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
			HOME_DIR = file.DirectoryName;
			if (!HOME_DIR.EndsWith("\\"))
				HOME_DIR += "\\";

			prefsFile = HOME_DIR + "/" + LUKE_PREFS_FILE;
		}

		public void Load() 
		{
			Load(prefsFile);
		}
    
		public void Load(string filename) 
		{
			try
			{
				XmlSerializer serializer = 
					new XmlSerializer(typeof(PrefsData));

				pd = (PrefsData)serializer.Deserialize(new StreamReader(filename));
				if (MruList == null)
					MruList = new ArrayList();
				if (MruList.Count > MruMaxSize)
					MruList.RemoveRange(MruMaxSize, MruList.Count - MruMaxSize);
			} 
			catch (Exception) 
			{
				// not found or corrupted, keep defaults
			}
		}
    
		public void AddToMruList(string val) 
		{
			if (MruList.Count < MruMaxSize && !MruList.Contains(val))
				MruList.Add(val);
		}
    
		public void Save()
		{
			XmlSerializer serializer = 
				new XmlSerializer(typeof(PrefsData));
			serializer.Serialize(new StreamWriter(prefsFile), pd);
		}

		public class PrefsData
		{
			public string LastPwd;
			public ArrayList MruList;
			[DefaultValueAttribute(10)]
			public int MruMaxSize;
			[DefaultValueAttribute(true)]
			public bool UseCompound;		

			public PrefsData()
			{
				MruMaxSize = 10;
				UseCompound = true;
			}
		}
	}
}
