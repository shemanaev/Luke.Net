using System;
using System.Collections;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Created on January 22, 2003, 12:13 PM
	/// </summary>
	public class IntPair
	{
		public int cnt = 0;
		public String text = null;
    
		public IntPair(int cnt, String text) 
		{
			this.cnt = cnt;
			this.text = text;
		}

		public override String ToString() 
		{
			return cnt + ":'" + text + "'";
		}
    
		public class PairComparator : IComparer
		{
			private bool ascending;
			private bool byText;
        
			public PairComparator(bool byText, bool ascending) 
			{
				this.ascending = ascending;
				this.byText = byText;
			}
        
			public int Compare(Object obj, Object obj1) 
			{
				IntPair h1 = (IntPair)obj;
				IntPair h2 = (IntPair)obj1;
				if (byText) 
				{
					return ascending ? h1.text.CompareTo(h2.text) : h2.text.CompareTo(h1.text);
				} 
				else 
				{
					if (h1.cnt > h2.cnt) return ascending ? -1 : 1;
					if (h1.cnt < h2.cnt) return ascending ? 1 : -1;
				}
				return 0;
			}
		}
	}
}
