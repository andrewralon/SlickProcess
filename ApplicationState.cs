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

		private string instruction;

		private ImageSource picture;

		private string picturePath;

		private string number;

		private string backText;

		private string nextText;

		private string cancelText;

		private bool backEnabled;

		private bool nextEnabled;

		#endregion Fields

		#region Properties

		public string Instruction
		{
			get { return instruction; }
			set
			{
				instruction = value;
				NotifyPropertyChanged("Instruction");
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

		public string BackText
		{
			get { return backText; }
			set
			{
				backText = value;
				NotifyPropertyChanged("BackText");
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

		public string CancelText
		{
			get { return cancelText; }
			set
			{
				cancelText = value;
				NotifyPropertyChanged("CancelText");
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

		public bool NextEnabled
		{
			get { return nextEnabled; }
			set
			{
				nextEnabled = value;
				NotifyPropertyChanged("NextEnabled");
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
