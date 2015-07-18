using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SlickProcess
{
	public class ApplicationState : INotifyPropertyChanged
	{
		#region Fields

		public event PropertyChangedEventHandler PropertyChanged;

		private ImageSource picture;

		private string instruction;

		private string stepNumber;

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

		public ImageSource Picture
		{
			get { return picture; }
			set
			{
				picture = value;
				NotifyPropertyChanged("Picture");
			}
		}

		public string StepNumber
		{
			get { return stepNumber; }
			set
			{
				stepNumber = value;
				NotifyPropertyChanged("StepNumber");
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

		#endregion Private Methods
	}
}
