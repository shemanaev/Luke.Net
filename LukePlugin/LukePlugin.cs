using System;
using System.Windows.Forms;

using Lucene.Net.Index;
using Lucene.Net.Store;

namespace Lucene.Net.LukeNet.Plugins
{

	/// <summary>
	/// LukePlugin is a superclass of any plugin that wants to
	/// be loaded automatically, and to work with the current index.
	/// <author>abial</author>
	/// </summary>
	public class LukePlugin : UserControl
	{
		protected Control myUi;
		protected IndexReader ir;
		protected Directory dir;
    
		/// <summary>
		/// Set a reference to the IndexReader currently open in
		/// the application.
		/// </summary>
		/// <param name="ir">IndexReader for the current index</param>
		public void SetIndexReader(IndexReader ir) 
		{
			this.ir = ir;
		}
    
		/// <summary>
		/// Returns a reference to the IndexReader currently open
		/// in the application.
		/// </summary>
		public IndexReader GetIndexReader() 
		{
			return ir;
		}
    
		public void SetDirectory(Directory dir) 
		{
			this.dir = dir;
		}
    
		public Directory GetDirectory() 
		{
			return dir;
		}
    
		/// <summary>
		/// Initialize this component. Parent view, this view,
		/// directory and index reader should already be initialized.
		/// <br>This method will be called repeatedly, whenever new
		/// index is loaded into Luke.
		/// </summary>
		/// <returns>true on success, false on non-catastrophic failure</returns>
		public virtual bool Init()
		{ return true; }
    
		/// <summary>
		/// Returns a plugin name. NOTE: this should be a short
		/// (preferably one word) String, because it's length affects
		/// the amount of available screen space. 
		/// </summary>
		/// <returns>short plugin name</returns>
		public virtual String GetPluginName()
		{ return ""; }
    
		/// <summary>
		/// Return short one-line info about the plugin.
		/// </summary>
		public virtual String GetPluginInfo()
		{ return ""; }
    
		/// <summary>
		///  Return URL to plugin home page or author's e-mail.
		/// NOTE: this MUST be a fully qualified URL, i.e. including
		/// the protocol part.
		/// </summary>
		public virtual String GetPluginHome()
		{ return ""; }
	}
}
