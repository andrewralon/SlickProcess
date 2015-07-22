using System.Windows.Media.Imaging;

namespace SlickProcess
{
	delegate bool StepMethod();

	class Step
	{
		#region Fields

		#endregion Fields

		#region Properties

		public string Instruction { get; set; }

		public string PicturePath { get; set; }

		public System.Windows.Media.Imaging.BitmapImage Picture { get; set; }

		public StepMethod StepMethod { get; set; }

		public bool Result { get; set; }

		#endregion Properties

		#region Constructors

		public Step(
			string instruction,
			string picturePath)
		{
			Instruction = instruction;
			PicturePath = picturePath;
			StepMethod = delegate() { return true; };
			Result = true;
		}

		public Step(
			string instruction,
			string picturePath,
			StepMethod stepMethod)
		{
			Instruction = instruction;
			PicturePath = picturePath;
			StepMethod = stepMethod;
			Result = false;
		}

		#endregion Constructors

		#region Public Methods

		#endregion Public Methods

		#region Private Methods

		#endregion Private Methods
	}
}
