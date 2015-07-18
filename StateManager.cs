using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SlickProcess
{
	public sealed class StateManager
	{
		#region Fields

		private static StateManager instance = new StateManager();

		#endregion Fields

		#region Properties

		public static StateManager Instance
		{
			get { return instance; }
		}

		public ApplicationState State { get; set; }

		private Step[] Steps { get; set; }

		private int CurrentStep { get; set; }

		#endregion Properties

		#region Constructors

		private StateManager()
		{
			int index = 1;
			Steps = new Step[]
			{
				new Step( // 1
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._01),
					StepOne), 
				new Step( // 2
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._02),
					StepTwo),
				new Step( // 3
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._03),
					StepThree),
				new Step( // 4
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._04),
					StepFour), 
				new Step( // 5
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._05),
					StepFive),
				new Step( // 6
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._06), 
					StepSix),
				new Step( // 7
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._07), 
					StepSeven),
				new Step( // 8
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._08), 
					StepEight),
				new Step( // 9
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._09), 
					StepNine),
				new Step( // 10
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._10), 
					StepTen),
				new Step( // 11
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._11),
					StepEleven),
				new Step( // 12
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._12)),
				new Step( // 13
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._13)),
				new Step( // 14
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._14), 
					StepFourteen),
				new Step( // 15
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._15)),
				new Step( // 16
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._16), 
					StepSixteen),
				new Step( // 17
					"Step " + (index++).ToString(), 
					Convert(Properties.Resources._17)),
				new Step( // 18
					"Complete! Step " + (index++).ToString(), 
					Convert(Properties.Resources._18),
					StepEighteen)
			};

			State = new ApplicationState();
			State.BackText = "Back";
			State.NextText = "Next";

			Transition(0);
		}

		#endregion Constructors

		#region Public Methods

		internal void Next()
		{
			Steps[CurrentStep].Result = ExecuteStep();
			int nextStep = Steps[CurrentStep].Result ? CurrentStep + 1 : FallBackStep();

			Transition(nextStep);
		}

		internal void Back()
		{
			Transition(CurrentStep - 1);
		}

		#endregion Public Methods

		#region Private Methods

		private void Transition(int nextStep)
		{
			CurrentStep = nextStep;

			State.BackEnabled = CurrentStep <= 0 ? false : true;
			State.NextEnabled = CurrentStep >= Steps.Length - 1 ? false : true;
			State.CancelText = CurrentStep >= Steps.Length - 1 ? "Finish" : "Cancel";

			Step step = Steps[CurrentStep];
			State.Instruction = step.Instruction;
			State.Picture = step.Picture;
			State.StepNumber = "Step " + (CurrentStep + 1) + " of " + Steps.Length;
		}

		private bool ExecuteStep()
		{
			return Steps[CurrentStep].StepMethod();
		}

		private int FallBackStep()
		{
			switch (CurrentStep)
			{
				//case 5:
				//case 6:
				//	return 5;
				default:
					return CurrentStep;
			}
		}

		private bool StepOne()
		{
			return true;
		}

		private bool StepTwo()
		{
			return true;
		}

		private bool StepThree()
		{
			return true;
		}

		private bool StepFour()
		{
			return true;
		}

		private bool StepFive()
		{
			return true;
		}

		private bool StepSix()
		{
			return true;
		}

		private bool StepSeven()
		{
			return true;
		}

		private bool StepEight()
		{
			return true;
		}

		private bool StepNine()
		{
			return true;
		}

		private bool StepTen()
		{
			return true;
		}

		private bool StepEleven()
		{
			return true;
		}

		private bool StepFourteen()
		{
			return true;
		}

		private bool StepSixteen()
		{
			return true;
		}

		private bool StepEighteen()
		{
			return true;
		}

		private static BitmapImage Convert(System.Drawing.Bitmap value)
		{
			MemoryStream ms = new MemoryStream();
			value.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

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
