using System.Drawing;
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

		public BitmapImage Picture { get; set; }

		public string Command { get; set; }

		public StepMethod Method { get; set; }

		public bool Passed { get; set; }

		public string Output { get; set; }

		public string Error { get; set; }

		#endregion Properties

		#region Constructors

		// Just instructions 
		public Step(string instruction)
		{
			SetDefaults(true);
			Instruction = instruction;
		}
		
		// Just instructions and a picture
		public Step(
			string instruction,
			string picturePath)
		{
			SetDefaults(true);
			Instruction = instruction;
			PicturePath = picturePath;
		}

		// Windows command determines passing criteria
		public Step(
			string instruction,
			string picturePath,
			string command)
		{
			SetDefaults();
			Instruction = instruction;
			PicturePath = picturePath;
			Command = command;
		}

		//// Saved image and Windows command
		//public Step(
		//	string instruction,
		//	string picturePath,
		//	byte[] picture,
		//	string command)
		//{
		//	SetDefaults();
		//	Instruction = instruction;
		//	PicturePath = picturePath;
		//	Picture = picture;
		//	Command = command;
		//}

		// Method determines passing criteria
		public Step(
			string instruction,
			string picturePath,
			StepMethod method)
		{
			SetDefaults();
			Instruction = instruction;
			PicturePath = picturePath;
			Method = method;
		}

		#endregion Constructors

		#region Public Methods

		#endregion Public Methods

		#region Private Methods

		public void SetDefaults(bool passed = false)
		{
			Instruction = "";
			PicturePath = "";
			Command = "";
			Method = delegate() { return true; };
			Passed = passed;
			Output = "";
			Error = "";
		}

		#endregion Private Methods
	}
}
