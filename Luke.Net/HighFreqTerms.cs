using System;
using System.Collections;

using Lucene.Net.Util;
using Lucene.Net.Store;
using Lucene.Net.Index;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// HighFreqTerms class extracts terms and their frequencies 
	/// out of an existing Lucene index.
	/// </summary>
	public class HighFreqTerms
	{
		private static int defaultNumTerms = 100;

		public static int DefaultNumTerms
		{
			get
			{ return defaultNumTerms; }
			set
			{ defaultNumTerms = value; }
		}
		
		public static TermInfo[] GetHighFreqTerms(Directory dir, 
												  Hashtable junkWords, 
												  String[] fields)
		{
			return GetHighFreqTerms(dir, junkWords, defaultNumTerms, fields);
		}

		public static TermInfo[] GetHighFreqTerms(Directory dir, 
												  Hashtable junkWords, 
												  int numTerms, 
												  String[] fields)
		{
			if (dir == null || fields == null) return new TermInfo[0];
        
			IndexReader reader = IndexReader.Open(dir, true);
			TermInfoQueue tiq = new TermInfoQueue(numTerms);
			TermEnum terms = reader.Terms();
        
			int minFreq = 0;
			
			while (terms.Next()) 
			{
				String field = terms.Term.Field;
            
				if (fields != null && fields.Length > 0) 
				{
					bool skip = true;
					
					for (int i = 0; i < fields.Length; i++) 
					{
						if (field.Equals(fields[i])) 
						{
							skip = false;
							break;
						}
					}
					if (skip) continue;
				}
            
				if (junkWords != null && junkWords[terms.Term.Text] != null)
					continue;
				
				if (terms.DocFreq() > minFreq) 
				{
					tiq.Add(new TermInfo(terms.Term, terms.DocFreq()));
					if (tiq.Size() >= numTerms) 		     // if tiq overfull
					{
						tiq.Pop();				     // remove lowest in tiq
						minFreq = ((TermInfo)tiq.Top()).DocFreq; // reset minFreq
					}
				}
			}
			
			TermInfo[] res = new TermInfo[tiq.Size()];
        
			for (int i = 0; i < res.Length; i++) 
			{
				res[res.Length - i - 1] = (TermInfo)tiq.Pop();
			}
			
			reader.Dispose();
			
			return res;
		}
	}

	sealed class TermInfoQueue : PriorityQueue<Object>
	{
		public TermInfoQueue(int size)
		{
			Initialize(size);
		}
	    
		public override bool LessThan(Object a, Object b)
		{
			TermInfo termInfoA = a as TermInfo;
			TermInfo termInfoB = b as TermInfo;
			
			if (null == termInfoA  || null == termInfoB)
				return false;
				
			return termInfoA.DocFreq < termInfoB.DocFreq;
		}
	}
}
