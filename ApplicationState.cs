using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SlickProcess
{
	public class ApplicationState : INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private string windowTitle;

		private string windowTag;

		private string instruction;

		//private Visibility instructionVisibility;

		private string instructionEdit;

		//private Visibility instructionEditVisibility;

		private ImageSource picture;

		private string picturePath;

		private string number;

		//private Visibility deleteStepButtonVisibility;

		//private Visibility deletePictureButtonVisibility;

		//private Visibility moveBackVisibility;

		private bool moveBackEnabled;

		//private Visibility moveNextVisibility;

		private bool moveNextEnabled;

		private string backText;

		private bool backEnabled;

		private string nextText;

		private bool nextEnabled;

		private string cancelText;

		private string command;

		//private Visibility commandVisibility;

		private string commandEdit;

		//private Visibility commandEditVisibility;

		private bool editMode;

		#endregion Fields

		#region Properties

		public string WindowTitle
		{
			get { return windowTitle; }
			set
			{
				windowTitle = value;
				NotifyPropertyChanged("WindowTitle");
			}
		}

		public string WindowTag
		{
			get { return windowTag; }
			set
			{
				windowTag = value;
				NotifyPropertyChanged("WindowTag");
			}
		}

		public string Instruction
		{
			get { return instruction; }
			set
			{
				instruction = value;
				instructionEdit = value;
				NotifyPropertyChanged("Instruction");
				NotifyPropertyChanged("InstructionEdit");
			}
		}

		public Visibility InstructionVisibility
		{
			get { return EditMode ? Visibility.Hidden : Visibility.Visible; }
		}

		public string InstructionEdit
		{
			get { return instructionEdit; }
			set
			{
				instructionEdit = value;
				instruction = value;
				NotifyPropertyChanged("InstructionEdit");
				NotifyPropertyChanged("Instruction");

				StateManager.Instance.UpdateSteps();
			}
		}

		public Visibility InstructionEditVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public string PicturePath
		{
			get { return picturePath; }
			set
			{
				picturePath = value;
				Picture = Convert(picturePath);
				//NotifyPropertyChanged("PicturePath");
			}
		}

		public ImageSource Picture
		{
			get { return picture; }
			set
			{
				picture = value;
				NotifyPropertyChanged("Picture");
			}
		}

		public string Number
		{
			get { return number; }
			set
			{
				number = value;
				NotifyPropertyChanged("Number");
			}
		}

		public Visibility DeleteStepButtonVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility DeletePictureButtonVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public Visibility MoveBackVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public bool MoveBackEnabled
		{
			//get { return EditMode ? true : false; }
			get { return moveBackEnabled; }
			set
			{
				moveBackEnabled = value;
				NotifyPropertyChanged("MoveBackEnabled");
			}
		}

		public Visibility MoveNextVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public bool MoveNextEnabled
		{
			//get { return EditMode ? true : false; }
			get { return moveNextEnabled; }
			set
			{
				moveNextEnabled = value;
				NotifyPropertyChanged("MoveNextEnabled");
			}
		}

		public string BackText
		{
			get { return backText; }
			set
			{
				backText = value;
				NotifyPropertyChanged("BackText");
			}
		}

		public bool BackEnabled
		{
			get { return backEnabled; }
			set
			{
				backEnabled = value;
				NotifyPropertyChanged("BackEnabled");
			}
		}

		public string NextText
		{
			get { return nextText; }
			set
			{
				nextText = value;
				NotifyPropertyChanged("NextText");
			}
		}

		public bool NextEnabled
		{
			get { return nextEnabled; }
			set
			{
				nextEnabled = value;
				NotifyPropertyChanged("NextEnabled");
			}
		}

		public string CancelText
		{
			get { return cancelText; }
			set
			{
				cancelText = value;
				NotifyPropertyChanged("CancelText");
			}
		}

		public string Command
		{
			get { return command; }
			set
			{
				command = value;
				commandEdit = value;
				NotifyPropertyChanged("Command");
				NotifyPropertyChanged("CommandEdit");
			}
		}

		public Visibility CommandVisibility
		{
			get { return EditMode ? Visibility.Hidden : Visibility.Visible; }
		}

		public string CommandEdit
		{
			get { return commandEdit; }
			set
			{
				commandEdit = value;
				command = value;
				NotifyPropertyChanged("CommandEdit");
				NotifyPropertyChanged("Command");

				StateManager.Instance.UpdateSteps();
			}
		}

		public Visibility CommandEditVisibility
		{
			get { return EditMode ? Visibility.Visible : Visibility.Hidden; }
		}

		public bool EditMode
		{
			get { return editMode; }
			set
			{
				editMode = value;
				NotifyPropertyChanged("EditMode");
				NotifyPropertyChanged("InstructionVisibility");
				NotifyPropertyChanged("InstructionEditVisibility");
				NotifyPropertyChanged("CommandVisibility");
				NotifyPropertyChanged("CommandEditVisibility");
				NotifyPropertyChanged("DeletePictureButtonVisibility");
				NotifyPropertyChanged("DeleteStepButtonVisibility");
				NotifyPropertyChanged("MoveBackEnabled");
				NotifyPropertyChanged("MoveBackVisibility");
				NotifyPropertyChanged("MoveNextEnabled");
				NotifyPropertyChanged("MoveNextVisibility");
			}
		}

		#endregion Properties

		#region Constructors

		#endregion Constructors

		#region Public Methods

		#endregion Public Methods

		#region Private Methods

		private void NotifyPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private static BitmapImage Convert(string bitmapPath)
		{
			if (bitmapPath == "")
			{
				return null;
			}

			MemoryStream ms = new MemoryStream();
			Bitmap bitmap = new Bitmap(bitmapPath);
			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			ms.Seek(0, SeekOrigin.Begin);
			image.StreamSource = ms;
			image.EndInit();

			return image;
		}

		#endregion Private Methods
	}
}
