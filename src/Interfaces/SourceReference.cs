using System;
using System.IO;

/// <summary>
/// Holds a reference to a specific part of source code
/// </summary>
namespace Devkit.Interfaces
{
	public class SourceReference
	{
		private readonly string file;

		private readonly int line;

		private readonly int offset;

		private readonly int length;

		private readonly int endLine;

		private readonly int endOffset;

		/// <summary>
		/// Gets the end line number
		/// </summary>
		public int EndLine
		{
			get
			{
				return this.endLine;
			}
		}

		/// <summary>
		/// Gets the end offset within the end line
		/// </summary>
		public int EndOffset
		{
			get
			{
				return this.endOffset;
			}
		}

		/// <summary>
		/// Gets the filename
		/// </summary>
		public string File
		{
			get
			{
				return this.file;
			}
		}

		/// <summary>
		/// Gets the length of the reference
		/// </summary>
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		/// <summary>
		/// Gets the line number
		/// </summary>
		public int Line
		{
			get
			{
				return this.line;
			}
		}

		/// <summary>
		/// Gets the offset within the line
		/// </summary>
		public int Offset
		{
			get
			{
				return this.offset;
			}
		}

		/// <summary>
		/// Creates a new instance of the source-reference class
		/// </summary>
		/// <param name="file"></param>
		/// <param name="line"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <param name="endLine"></param>
		/// <param name="endOffset"></param>
		public SourceReference(string file, int line, int offset, int length, int endLine, int endOffset)
		{
			this.file = file;
			this.line = line;
			this.offset = offset;
			this.length = length;
			this.endLine = endLine;
			this.endOffset = endOffset;
		}

		public override bool Equals(object obj)
		{
			SourceReference that = obj as SourceReference;
			if (that != null)
			{
				if (!object.ReferenceEquals(this, obj))
				{
					if (this.file == that.file)
					{
						if (this.line == that.line)
						{
							if (this.offset == that.offset)
							{
								if (this.length == that.length)
								{
									if (this.endLine == that.endLine)
									{
										if (this.endOffset == that.endOffset)
										{
											return true;
										}
										else
										{
											return false;
										}
									}
									else
									{
										return false;
									}
								}
								else
								{
									return false;
								}
							}
							else
							{
								return false;
							}
						}
						else
						{
							return false;
						}
					}
					else
					{
						return false;
					}
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Extends a source reference to have the ending of the specified source reference
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public SourceReference ExtendBy(SourceReference other)
		{
			return new SourceReference(this.file, this.line, this.offset, this.length + other.length, other.endLine, other.endOffset);
		}

		public override int GetHashCode()
		{
			return this.file.GetHashCode() + this.line + this.offset + this.length + this.endLine + this.endOffset;
		}

		public override string ToString()
		{
			object[] fileName = new object[5];
			fileName[0] = Path.GetFileName(this.file);
			fileName[1] = ":";
			fileName[2] = this.line;
			fileName[3] = " offset ";
			fileName[4] = this.offset;
			return string.Concat(fileName);
		}
	}
}