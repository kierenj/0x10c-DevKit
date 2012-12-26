using SmartAssembly.SmartExceptionsCore;
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Devkit.IDE.Resources
{
	public class Images
	{
		public static BitmapImage BreakpointOnIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/breakpoint_on_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage BuildIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/build_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage CopyIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/copy_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage Cross
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/cross.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage CutIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/cut_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage DeleteIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage DisplayFont
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/font.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileAddIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_add_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileCloseIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileDeleteIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileNewIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_add_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FileOpenIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/file_open_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage FolderOpenIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/folder_open_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage NavigateBackIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/navigate_back_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage NavigateForwardIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/navigate_forward_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage PasteIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/paste_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage PauseIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/test_pause_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ProjectAddIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/project_add_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ProjectCloseIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/project_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ProjectDeleteIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/project_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ProjectIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/project_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ProjectOpenIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/project_open_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage RedoIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/redo_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage RefreshIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/refresh_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage RunIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/test_run_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SaveAllIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/save_all_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SaveIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/save_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage ShowNextStatementIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/test_show_next_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionCloseIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionDeleteIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_delete_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionNewIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_new_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionNewLargeIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_new_32.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionOpenIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/solution_open_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage SolutionOpenLargeIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/folder_open_32.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage StepIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/test_step_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage StopIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/test_stop_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage Tick
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/tick.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public static BitmapImage UndoIcon
		{
			get
			{
				BitmapImage image;
				try
				{
					image = Images.GetImage(new Uri("pack://application:,,,/Resources/Images/undo_16.png"));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
				return image;
			}
		}

		public Images()
		{
		}

		public static BitmapImage GetDisabledImage(Uri uri)
		{
			BitmapImage bitmapImage;
			FormatConvertedBitmap formatConvertedBitmap;
			PngBitmapEncoder pngBitmapEncoder;
			MemoryStream memoryStream;
			BitmapImage bitmapImage1;
			BitmapImage bitmapImage2;
			BitmapImage bitmapImage3;
			try
			{
				if (uri != null)
				{
					try
					{
						bitmapImage = new BitmapImage();
						bitmapImage.BeginInit();
						bitmapImage.UriSource = uri;
						bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
						bitmapImage.EndInit();
						bitmapImage.Freeze();
						formatConvertedBitmap = new FormatConvertedBitmap();
						formatConvertedBitmap.BeginInit();
						formatConvertedBitmap.Source = bitmapImage;
						formatConvertedBitmap.DestinationFormat = PixelFormats.Gray32Float;
						formatConvertedBitmap.EndInit();
						pngBitmapEncoder = new PngBitmapEncoder();
						memoryStream = new MemoryStream();
						bitmapImage1 = new BitmapImage();
						pngBitmapEncoder.Frames.Add(BitmapFrame.Create(formatConvertedBitmap));
						pngBitmapEncoder.Save(memoryStream);
						bitmapImage1.BeginInit();
						bitmapImage1.StreamSource = new MemoryStream(memoryStream.ToArray());
						bitmapImage1.EndInit();
						memoryStream.Close();
						bitmapImage2 = bitmapImage1;
					}
					catch (Exception exception)
					{
						bitmapImage2 = null;
					}
					bitmapImage3 = bitmapImage2;
				}
				else
				{
					bitmapImage3 = null;
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException7(exception1, bitmapImage, formatConvertedBitmap, pngBitmapEncoder, memoryStream, bitmapImage1, bitmapImage2, uri);
				throw;
			}
			return bitmapImage3;
		}

		public static BitmapImage GetImage(Uri uri)
		{
			BitmapImage bitmapImage;
			BitmapImage bitmapImage1;
			BitmapImage bitmapImage2;
			try
			{
				if (uri != null)
				{
					try
					{
						bitmapImage = new BitmapImage();
						bitmapImage.BeginInit();
						bitmapImage.UriSource = uri;
						bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
						bitmapImage.EndInit();
						bitmapImage.Freeze();
						bitmapImage1 = bitmapImage;
					}
					catch (Exception exception)
					{
						bitmapImage1 = null;
					}
					bitmapImage2 = bitmapImage1;
				}
				else
				{
					bitmapImage2 = null;
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException3(exception1, bitmapImage, bitmapImage1, uri);
				throw;
			}
			return bitmapImage2;
		}
	}
}