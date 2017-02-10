using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace Lucene.Net.LukeNet
{
	/// <summary>
	/// Styles used in the FolderBrowser Dialog
	/// </summary>
	[Flags]    
	public enum FolderBrowserStyles 
	{
		RestrictToFilesystem = 0x0001, // BIF_RETURNONLYFSDIRS
		RestrictToDomain =     0x0002, // BIF_DONTGOBELOWDOMAIN
		RestrictToSubfolders = 0x0008, // BIF_RETURNFSANCESTORS
		ShowTextBox =          0x0010, // BIF_EDITBOX
		ValidateSelection =    0x0020, // BIF_VALIDATE
		NewDialogStyle =       0x0040, // BIF_NEWDIALOGSTYLE
		BrowseForComputer =    0x1000, // BIF_BROWSEFORCOMPUTER
		BrowseForPrinter =     0x2000, // BIF_BROWSEFORPRINTER
		BrowseForEverything =  0x4000, // BIF_BROWSEINCLUDEFILES
	}

	/// <summary>
	/// Enum of CSIDLs identifying standard shell folders.
	/// </summary>
	public enum FolderID 
	{
		Desktop                   = 0x0000,
		Printers                  = 0x0004,
		MyDocuments               = 0x0005,
		Favorites                 = 0x0006,
		Recent                    = 0x0008,
		SendTo                    = 0x0009,
		StartMenu                 = 0x000b,
		MyComputer                = 0x0011,
		NetworkNeighborhood       = 0x0012,
		Templates                 = 0x0015,
		MyPictures                = 0x0027,
		NetAndDialUpConnections   = 0x0031,
	}

	public sealed class FolderBrowser : Component
	{
		private static readonly int MAX_PATH = 260;
    
		// Root node of the tree view.
		private FolderID startLocation = FolderID.Desktop;

		// Browse info options.
		private int publicOptions = (int) FolderBrowserStyles.RestrictToFilesystem | 
									(int) FolderBrowserStyles.RestrictToDomain;
		private int privateOptions = (int) FolderBrowserStyles.NewDialogStyle;

		// Description text to show.
		private string descriptionText = "Please select a folder below:";

		// Folder chosen by the user.
		private string directoryPath = String.Empty;

		/// <summary>
		/// Helper function that returns the IMalloc interface used by the shell.
		/// </summary>
		private static Win32Api.IMalloc GetSHMalloc ( )
		{
			Win32Api.IMalloc malloc;
			Win32Api.Shell32.SHGetMalloc ( out malloc );
			return malloc;
		}

		/// <summary>
		/// Shows the folder browser dialog box.
		/// </summary>
		public DialogResult ShowDialog ( ) 
		{
			return ShowDialog ( null );
		}

		/// <summary>
		/// Shows the folder browser dialog box with the specified owner window.
		/// </summary>
		public DialogResult ShowDialog ( IWin32Window owner ) 
		{
			IntPtr pidlRoot = IntPtr.Zero;

			// Get/find an owner HWND for this dialog.
			IntPtr hWndOwner;

			if ( owner != null ) 
			{
				hWndOwner = owner.Handle;
			}
			else 
			{
				hWndOwner = Win32Api.GetActiveWindow ( );
			}

			// Get the IDL for the specific startLocation.
			Win32Api.Shell32.SHGetSpecialFolderLocation ( hWndOwner, (int) startLocation, out pidlRoot );

			if (pidlRoot == IntPtr.Zero) 
			{
				return DialogResult.Cancel;
			}

			int mergedOptions = (int) publicOptions | privateOptions;

			if ( ( mergedOptions & (int) FolderBrowserStyles.NewDialogStyle ) != 0 ) 
			{
				if ( System.Threading.ApartmentState.MTA == Application.OleRequired ( ) )
					mergedOptions = mergedOptions & (~ (int) FolderBrowserStyles.NewDialogStyle);
			}

			IntPtr pidlRet = IntPtr.Zero;

			try 
			{
				// Construct a BROWSEINFO.
				Win32Api.Shell32.BROWSEINFO bi = new Win32Api.Shell32.BROWSEINFO ( );
				IntPtr buffer = Marshal.AllocHGlobal ( MAX_PATH);

				bi.pidlRoot = pidlRoot;
				bi.hwndOwner = hWndOwner;
				bi.pszDisplayName = buffer;
				bi.lpszTitle = descriptionText;
				bi.ulFlags = mergedOptions;
				// The rest of the fields are initialized to zero by the constructor.
				// bi.lpfn = null;  bi.lParam = IntPtr.Zero;    bi.iImage = 0;

				// Show the dialog.
				pidlRet = Win32Api.Shell32.SHBrowseForFolder ( ref bi );

				// Free the buffer you've allocated on the global heap.
				Marshal.FreeHGlobal ( buffer );

				if ( pidlRet == IntPtr.Zero ) 
				{
					// User clicked Cancel.
					return DialogResult.Cancel;
				}

				// Then retrieve the path from the IDList.
				StringBuilder sb = new StringBuilder ( MAX_PATH );
				if ( 0 == Win32Api.Shell32.SHGetPathFromIDList ( pidlRet, sb ) )
				{
					return DialogResult.Cancel;
				}

				// Convert to a string.
				directoryPath = sb.ToString ( );
			}
			finally 
			{
				Win32Api.IMalloc malloc = GetSHMalloc ( );
				malloc.Free ( pidlRoot );

				if ( pidlRet != IntPtr.Zero ) 
				{
					malloc.Free ( pidlRet );
				}
			}

			return DialogResult.OK;
		} 
	
		/// <summary>
		/// Helper function used to set and reset bits in the publicOptions bitfield.
		/// </summary>
		private void SetOptionField ( int mask, bool turnOn )
		{
			if (turnOn)
				publicOptions |= mask;
			else
				publicOptions &= ~mask;
		}

		/// <summary>
		/// Only return file system directories. If the user selects folders
		/// that are not part of the file system, the OK button is unavailable.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Only return file system directories. If the user selects folders " +
			 "that are not part of the file system, the OK button is unavailable." )]
		[DefaultValue ( true )]
		public bool OnlyFilesystem
		{
			get 
			{
				return (publicOptions & (int) FolderBrowserStyles.RestrictToFilesystem) != 0;
			}
			set 
			{
				SetOptionField ( (int) FolderBrowserStyles.RestrictToFilesystem, value );
			}
		}

		/// <summary>
		/// Location of the root folder from which to start browsing. Only the specified
		/// folder and any folders beneath it in the namespace hierarchy  appear
		/// in the dialog box.
		/// </summary>
		[Category ( "Navigation" )]
		[Description ( "Location of the root folder from which to start browsing. Only the specified " +
			 "folder and any folders beneath it in the namespace hierarchy appear " +
			 "in the dialog box." )]
		[DefaultValue ( typeof(FolderID), "0")]
		public FolderID StartLocation 
		{
			get 
			{
				return startLocation;
			}
			set 
			{
				new UIPermission ( UIPermissionWindow.AllWindows ).Demand ( );
				startLocation = value;
			}
		}

		/// <summary>
		/// Full path to the folder selected by the user.
		/// </summary>
		[Category("Navigation")]
		[Description("Full path to the folder slected by the user.")]
		public string DirectoryPath 
		{
			get 
			{
				return directoryPath;
			}
		}
	
		/// <summary>
		/// Description text to show.
		/// </summary>
		[Category("Navigation")]
		[Description("Description text to show.")]
		public string DescriptionText
		{
			get
			{
				return descriptionText;
			}
			set
			{
				descriptionText = value;
			}
		} 

	}
}