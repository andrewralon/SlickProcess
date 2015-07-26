using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		// Debug / proof of concept / testing variables
		private string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		private string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\pics\\";
		private string picPath = "\\..\\..\\Resources\\";
		private string xmlPath = "steps.xml";
		private string fullPath;

		#endregion Fields

		#region Properties

		public static StateManager Instance
		{
			get { return instance; }
		}

		public ApplicationState State { get; set; }

		public string ProcessName { get; set; }

		private List<Step> Steps { get; set; }

		private int CurrentStep { get; set; }

		#endregion Properties

		#region Constructors

		private StateManager()
		{
			ProcessName = "";
		}

		#endregion Constructors

		#region Public Methods

		internal void Demo()
		{
			// Get list of pictures in Resources folder
			fullPath = Path.GetFullPath(path + picPath);
			string[] pics = Directory.GetFiles(fullPath, "*.jpg");

			// Create an XML file from the available pictures
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.NewLineOnAttributes = true;

			using (XmlWriter writer = XmlWriter.Create(xmlPath, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("SlickProcess");
				writer.WriteElementString("Version", Application.ResourceAssembly.GetName().Version.ToString());

				for (int i = 0; i < pics.Length; i++)
				{
					writer.WriteStartElement("Step");

					writer.WriteElementString("Instruction", "Instruction for step " + (i + 1).ToString());
					writer.WriteElementString("PicturePath", pics[i].ToString());
					writer.WriteElementString("Command", "copy \"" + pics[i].ToString() + "\" \"" + desktop + "\"");

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}

			if (!Directory.Exists(desktop))
			{
				Directory.CreateDirectory(desktop);
			}

			// DEBUG - Delete the existing files in the desktop folder
			foreach (string filePath in Directory.GetFiles(desktop))
			{
				File.Delete(filePath);
			}

			Open(xmlPath);
		}

		internal void Open(string filePath)
		{
			string filename = Path.GetFileName(filePath);
			if (ProcessName == "")
			{
				ProcessName = filePath;
			}
			else
			{

				MessageBoxResult result = 
					MessageBox.Show("Would you like to open \"" + filename + "\"?", "Open a Process", MessageBoxButton.YesNo);
				if (result != MessageBoxResult.Yes)
				{
					return;
				}
			}

			// Load the XML file
			XDocument xml = XDocument.Load(filePath);

			// Create the list of steps from the parsed data
			//  Not sure what to do with Method yet....
			Steps = (from c in xml.Root.Descendants("Step")
					 select new Step(
						 c.Element("Instruction").Value,
						 c.Element("PicturePath").Value,
						 c.Element("Command").Value
					)).ToList();

			// Set the defaults on the GUI
			State = new ApplicationState();
			State.WindowTitle = filename;
			State.BackText = "Back";
			State.NextText = "Next";

			// Transition to the first state
			Transition(0);
		}

		internal void Next()
		{
			// Run the step's method and store the result
			Steps[CurrentStep].Passed = ExecuteStep();

			// What's the next step? If it passed, add 1. If it failed, display a message and go to the FallBackStep.
			if (Steps[CurrentStep].Passed)
			{
				Transition(CurrentStep + 1);
			}
			else
			{
				int fallBackStep = FallBackStep();

				// Gather all useful information to tell the user why the step failed
				string message = "The following command failed:" + Environment.NewLine +
					">" + Steps[CurrentStep].Command + Environment.NewLine + Environment.NewLine;

				// Standard error and output, just like in the Windows console
				message += Steps[CurrentStep].Error + Steps[CurrentStep].Output;

				// Tell the user which step is next after failing
				message += Environment.NewLine + "Please fix the problem before continuing on to Step #" + (fallBackStep + 1) + "....";

				MessageBox.Show(message, "Step #" + (CurrentStep + 1) + " Failed");

				Transition(FallBackStep());
			}
		}

		internal void Back()
		{
			Transition(CurrentStep - 1);
		}

		#endregion Public Methods

		#region Private Methods

		private void Transition(int nextStep)
		{
			if (nextStep < 0 || nextStep > Steps.Count)
			{
				MessageBox.Show("There is no step number " + nextStep + "!", "Error Loading Step");
				return;
			}

			CurrentStep = nextStep;

			// Update the GUI with a new instruction, picture, and number
			State.Instruction = Steps[CurrentStep].Instruction;
			State.Number = "Step " + (CurrentStep + 1) + " of " + Steps.Count;
			State.PicturePath = Steps[CurrentStep].PicturePath;

			// Update the buttons for first and last steps
			State.BackEnabled = CurrentStep <= 0 ? false : true;
			State.NextEnabled = CurrentStep >= Steps.Count - 1 ? false : true;
			State.CancelText = CurrentStep >= Steps.Count - 1 ? "Finish" : "Cancel";
		}

		private bool ExecuteStep()
		{
			return RunCommand(Steps[CurrentStep].Command);
			//return Steps[CurrentStep].Method();
		}

		private int FallBackStep()
		{
			switch (CurrentStep)
			{
				// Example: Go back to step 5 when either step 5 or 6 fails
				//case 5:
				//case 6:
				//	return 5;
				// Otherwise, just do the current step over again if it fails
				default:
					return CurrentStep;
			}
		}

		private bool RunCommand(string command)
		{
			bool result = false;

			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();

			// Redirect standard output so we can show the command's result in the console
			startInfo.RedirectStandardOutput = true;
			startInfo.RedirectStandardError = true;
			startInfo.UseShellExecute = false;
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C " + command;

			// TODO - Debug only
			Console.WriteLine(">" + command);

			process.StartInfo = startInfo;
			process.Start();

			do
			{
				if (!process.HasExited)
				{
					process.Refresh();
				}
			}
			while (!process.WaitForExit(500));

			// TODO - Debug only
			string error = process.StandardError.ReadToEnd();
			Steps[CurrentStep].Error = error;
			Console.Write(error);

			string output = process.StandardOutput.ReadToEnd();
			Steps[CurrentStep].Output = output;
			Console.Write(output);

			Console.WriteLine("Process exit code: " + process.ExitCode);

			// Return true if there were no errors completing the command
			if (process.ExitCode == 0)
			{
				result = true;
			}

			if (process != null)
			{
				process.Close();
			}

			return result;
		}

		#endregion Private Methods
	}
}
