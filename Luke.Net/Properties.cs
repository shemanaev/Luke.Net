using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Java-style property management
	/// </summary>
	public class Properties
	{
		// /[^=:\s]+./
		private NameValueCollection props;

		public void Load(StreamReader stream)
		{
			props = new NameValueCollection();
			string line;
			char separators = new char[]{'=', ' ' , ':'};

			while ((line = stream.ReadLine()) != null)
			{
				if (line.Length == 0) continue;
				// comment
				if (line.StartsWith("#")) continue;

				line = line.TrimStart(separators).TrimEnd();
				
				// multiline property
				while (line.EndsWith("\\"))
				{
					line = line.Remove(line.Length - 1, 1);
					string line1;
					if ((line1 = stream.ReadLine()) != null)
						line += line1.Trim();
				}

				if (line.Length < 2) continue;

				// split property and value by first "=", " ", ":"
				StringBuilder key = new StringBuilder();

				for (int i = 1; i < line.Length; i++)
				{
					if (line[i] == '=' || line[i] == ':' || line[i] == ' ')
					{
						if (line[i - 1] == "\\")
						{
							if (i > 2)
								key.Append(line.Substring(0, i-2));
							key.Append(line.Substring(i));
						}
					}
				}

				int separator;

				while(true)
				{
					separator = line.IndexOfAny(new char[]{'=', ' ' , ':'});
				}

				string[] pair = line.Split(new char[]{'=', ' ' , ':'}, 2);
				if (pair.Length != 2) continue;

				props.Add(pair[0], pair[1]);
			}
		}

		public void Save(StreamWriter stream, string header)
		{
		
		}
	}
}
