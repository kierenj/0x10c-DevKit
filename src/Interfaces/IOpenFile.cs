using Devkit.Interfaces.Build;
using System;

/// <summary>
/// Represents a file which is open in the workspace
/// </summary>
namespace Devkit.Interfaces
{
	public interface IOpenFile
	{
		/// <summary>
		/// Gets the absolute path of the file
		/// </summary>
		string AbsolutePath
		{
			get;
		}

		/// <summary>
		/// Gets or sets the textual content of the file
		/// </summary>
		string Content
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the display title of the file
		/// </summary>
		string DisplayTitle
		{
			get;
		}

		/// <summary>
		/// Gets the underlying file object (represents the file in the project)
		/// </summary>
		IFile File
		{
			get;
		}

		/// <summary>
		/// Indicates whether the file is the currently-selected file
		/// </summary>
		bool IsCurrentFile
		{
			get;
		}

		/// <summary>
		/// Gets or sets the dirty flag (indicating whether the file has unsaved changes or not).
		/// May be set by editors which manually save non-textual content of a file.
		/// </summary>
		bool IsDirty
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a flag controlling whether the file is open and remains open
		/// </summary>
		bool IsOpen
		{
			get;
			set;
		}

		/// <summary>
		/// Causes the textual contents of the file to be saved to disk
		/// </summary>
		void Save();
	}
}