using System;
using System.Text; 
using System.Runtime.InteropServices;

namespace Lucene.Net.LukeNet
{
	internal class Win32Api
	{
		// C# representation of the IMalloc interface.
		[InterfaceType ( ComInterfaceType.InterfaceIsIUnknown ),
			Guid ( "00000002-0000-0000-C000-000000000046" )]
			public interface IMalloc
		{
			[PreserveSig] IntPtr Alloc ( [In] int cb );
			[PreserveSig] IntPtr Realloc ( [In] IntPtr pv, [In] int cb );
			[PreserveSig] void   Free ( [In] IntPtr pv );
			[PreserveSig] int    GetSize ( [In] IntPtr pv );
			[PreserveSig] int    DidAlloc ( IntPtr pv );
			[PreserveSig] void   HeapMinimize ( );
		}

		[DllImport("User32.DLL")]
		public static extern IntPtr GetActiveWindow ( );

		public class Shell32
		{

			// Delegate type used in BROWSEINFO.lpfn field.
			public delegate int BFFCALLBACK ( IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData );

			[StructLayout ( LayoutKind.Sequential, Pack=8 )]
			public struct BROWSEINFO
			{
				public IntPtr       hwndOwner;
				public IntPtr       pidlRoot;
				public IntPtr       pszDisplayName;
				[MarshalAs ( UnmanagedType.LPTStr )]
				public string       lpszTitle;
				public int          ulFlags;
				[MarshalAs ( UnmanagedType.FunctionPtr )]
				public BFFCALLBACK  lpfn;
				public IntPtr       lParam;
				public int          iImage;
			}

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetMalloc ( out IMalloc ppMalloc );

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetSpecialFolderLocation ( 
				IntPtr hwndOwner, int nFolder, out IntPtr ppidl );

			[DllImport ( "Shell32.DLL" )]
			public static extern int SHGetPathFromIDList ( 
				IntPtr pidl, StringBuilder Path );

			[DllImport ( "Shell32.DLL", CharSet=CharSet.Auto )]
			public static extern IntPtr SHBrowseForFolder ( ref BROWSEINFO bi );
		}
	} 
}