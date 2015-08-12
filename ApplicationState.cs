using System.ComponentModel;
using System.Drawing;
using System.IO;
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

        private WtfVisibility instructionVisibility;

		private string instructionEdit;

        private WtfVisibility instructionEditVisibility;

		private ImageSource picture;

		private string picturePath;

		private string number;

        private WtfVisibility deleteStepButtonVisibility;

        private WtfVisibility deletePictureButtonVisibility;

        private WtfVisibility moveBackVisibility;

		private bool moveBackEnabled;

        private WtfVisibility moveNextVisibility;

		private bool moveNextEnabled;

		private string backText;

		private bool backEnabled;

		private string nextText;

		private bool nextEnabled;

		private string cancelText;

		private string command;

		private WtfVisibility commandVisibility;

		private string commandEdit;

		private WtfVisibility commandEditVisibility;

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

        public WtfVisibility InstructionVisibility
		{
			get { return instructionVisibility; }
			set
			{
				instructionVisibility = value;
				NotifyPropertyChanged("InstructionVisibility");
			}
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

        public WtfVisibility InstructionEditVisibility
		{
			get { return instructionEditVisibility; }
			set
			{
				instructionEditVisibility = value;
				NotifyPropertyChanged("InstructionEditVisibility");
			}
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

		public WtfVisibility DeleteStepButtonVisibility
		{
			get { return deleteStepButtonVisibility; }
			set
			{
				deleteStepButtonVisibility = value;
				NotifyPropertyChanged("DeleteStepButtonVisibility");
			}
		}

        public WtfVisibility DeletePictureButtonVisibility
		{
			get { return deletePictureButtonVisibility; }
			set
			{
				deletePictureButtonVisibility = value;
				NotifyPropertyChanged("DeletePictureButtonVisibility");
			}
		}

        public WtfVisibility MoveBackVisibility
		{
			get { return moveBackVisibility; }
			set
			{
				moveBackVisibility = value;
				NotifyPropertyChanged("MoveBackVisibility");
			}
		}

		public bool MoveBackEnabled
		{
			get { return moveBackEnabled; }
			set
			{
				moveBackEnabled = value;
				NotifyPropertyChanged("MoveBackEnabled");
			}
		}

        public WtfVisibility MoveNextVisibility
		{
			get { return moveNextVisibility; }
			set
			{
				moveNextVisibility = value;
				NotifyPropertyChanged("MoveNextVisibility");
			}
		}

		public bool MoveNextEnabled
		{
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

		public WtfVisibility CommandVisibility
		{
			get { return commandVisibility; }
			set
			{
				commandVisibility = value;
				NotifyPropertyChanged("CommandVisibility");
			}
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

		public WtfVisibility CommandEditVisibility
		{
			get { return commandEditVisibility; }
			set
			{
				commandEditVisibility = value;
				NotifyPropertyChanged("CommandEditVisibility");
			}
		}

		public bool EditMode
		{
			get { return editMode; }
			set
			{
				editMode = value;
				NotifyPropertyChanged("EditMode");
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
