using Lucene.Net.Index;

namespace Lucene.Net.LukeNet
{
	public class TermInfo 
	{
		public TermInfo(Term term, int docFrequency) 
		{
			this.term = term;
			docFreq = docFrequency;
		}
		
		public TermInfo(TermInfo ti) 
		{
			docFreq = ti.docFreq;
			term = ti.Term;
		}

		
		private int docFreq;
		private Term term;
		
		public int DocFreq
		{
			set
			{ docFreq = value; }
			get
			{ return docFreq; }
		}
		
		public Term Term
		{
			set
			{ term = value; }
			get
			{ return term; }
		}
	}
}