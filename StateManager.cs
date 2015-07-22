using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Linq;

namespace SlickProcess
{
	public sealed class StateManager
	{
		#region Fields

		private static StateManager instance = new StateManager();

		private string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		private string picPath = "\\..\\..\\Resources\\";
		private string fullPath;

		#endregion Fields

		#region Properties

		public static StateManager Instance
		{
			get { return instance; }
		}

		public ApplicationState State { get; set; }

		private List<Step> Steps { get; set; }

		private int CurrentStep { get; set; }

		#endregion Properties

		#region Constructors

		private StateManager()
		{
			// Get list of pictures in Resources folder
			fullPath = Path.GetFullPath(path + picPath);
			string[] pics = Directory.GetFiles(fullPath, "*.jpg");

			// Create an XML file from the available pictures
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.NewLineOnAttributes = true;

			using (XmlWriter writer = XmlWriter.Create("steps.xml", settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("SlickProcess");

				for (int i = 0; i < pics.Length; i++)
				{
					writer.WriteStartElement("Step");

					writer.WriteElementString("Instruction", "Instruction for step " + (i + 1).ToString());
					writer.WriteElementString("PicturePath", pics[i].ToString());
					writer.WriteElementString("Method", true.ToString());

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}

			// Load the XML from the file
			XDocument xml = XDocument.Load("steps.xml");

			// Query the data and get the list of steps
			Steps = (from c in xml.Root.Descendants("Step")
					 select new Step(
						 c.Element("Instruction").Value,
						 c.Element("PicturePath").Value
					)).ToList();

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
			State.NextEnabled = CurrentStep >= Steps.Count - 1 ? false : true;
			State.CancelText = CurrentStep >= Steps.Count - 1 ? "Finish" : "Cancel";

			Step step = Steps[CurrentStep];
			State.Instruction = step.Instruction;
			State.PicturePath = step.PicturePath;
			State.StepNumber = "Step " + (CurrentStep + 1) + " of " + Steps.Count;
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

		#endregion Private Methods
	}
}
