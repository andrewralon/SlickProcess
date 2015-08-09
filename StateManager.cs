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
using Microsoft.Win32;

namespace SlickProcess
{
	public sealed class StateManager
	{
		#region Fields

		private static StateManager instance = new StateManager();
		private bool editMode = false;

		// Debug / proof of concept / testing variables
		private string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		private string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\SlickProcess\\";
		private string picPath = "\\..\\..\\Resources\\";
		private string xmlPath = "process1.xml";
		private string fullPath;

		#endregion Fields

		#region Properties

		public static StateManager Instance
		{
			get { return instance; }
		}

		public ApplicationState State { get; set; }

		public string ProcessName { get; set; }

		public string ProcessPath { get; set; }

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

					writer.WriteElementString("Instruction", "Copy picture from step " + (i + 1).ToString());
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
			string extension = Path.GetExtension(filePath);
			if (ProcessName == "")
			{
				ProcessName = filename;
				ProcessPath = filePath;
			}
			else if (extension != ".xml") // TODO - Check if the file is a valid process
			{
				return;
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
			State.BackText = "Back";
			State.NextText = "Next";

			// Transition to the first state
			Transition(0);
		}

		internal void Save(string filePath)
		{
			// TODO - Check if anything has changed w/ a "dirty" bool first!

			MessageBoxResult result = MessageBox.Show("Would you like to save changes to your process?",
				"Save your process?", MessageBoxButton.YesNo);
			if (result != MessageBoxResult.Yes)
			{
				return;
			}

			if (filePath == "")
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.InitialDirectory = desktop; // TODO - Make this a better location like My Documents
				sfd.Filter = "XML Files | *.xml";
				sfd.DefaultExt = "xml";
				sfd.ShowDialog();

				filePath = sfd.FileName;
				//filePath = xmlPath;
			}

			// Create an XML file from the available pictures
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			settings.NewLineOnAttributes = true;

			using (XmlWriter writer = XmlWriter.Create(filePath, settings))
			{
				writer.WriteStartDocument();
				writer.WriteStartElement("SlickProcess");
				writer.WriteElementString("Version", Application.ResourceAssembly.GetName().Version.ToString());

				for (int i = 0; i < Steps.Count; i++)
				{
					writer.WriteStartElement("Step");

					writer.WriteElementString("Instruction", Steps[i].Instruction);
					writer.WriteElementString("PicturePath", Steps[i].PicturePath);
					writer.WriteElementString("Command", Steps[i].Command);

					writer.WriteEndElement();
				}

				writer.WriteEndElement();
				writer.WriteEndDocument();
			}
		}

		internal void Back()
		{
			Transition(CurrentStep - 1);
		}

		internal void Next()
		{
			if (CurrentStep == Steps.Count - 1)
			{
				if (editMode)
				{
					Steps[CurrentStep].Passed = false;
					NewStep();
					Transition(CurrentStep + 1);
				}

				return;
			}

			if (editMode)
			{
				Steps[CurrentStep].Passed = false;
				Transition(CurrentStep + 1);
				return;
			}
			else
			{
				// Run the step's method and store the result
				Steps[CurrentStep].Passed = ExecuteStep();
			}

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

				Transition(fallBackStep);
			}
		}

		internal bool Close()
		{
			if (CurrentStep < Steps.Count - 1)
			{
				MessageBoxResult result = MessageBox.Show("Are you sure you want to close before finishing the current process?",
						"Are you sure?", MessageBoxButton.YesNo);

				if (result == MessageBoxResult.No)
				{
					return false;
				}
			}

			Save("");

			return true;
		}


		internal void ToggleEditMode(bool setEditMode)
		{
			editMode = setEditMode;

			if (editMode)
			{
				State.NextEnabled = true;

				State.InstructionEditVisibility = WtfVisibility.Visible;
				State.InstructionVisibility = WtfVisibility.Hidden;
				State.DeleteStepButtonVisibility = WtfVisibility.Visible;
                State.DeletePictureButtonVisibility = WtfVisibility.Visible;
                State.MoveBackVisibility = WtfVisibility.Visible;
                State.MoveNextVisibility = WtfVisibility.Visible;
			}
			else
			{
				if (CurrentStep >= Steps.Count - 1)
				{
					State.NextEnabled = false;
				}

                State.InstructionVisibility = WtfVisibility.Visible;
                State.InstructionEditVisibility = WtfVisibility.Hidden;
				State.DeleteStepButtonVisibility = WtfVisibility.Hidden;
				State.DeletePictureButtonVisibility = WtfVisibility.Hidden;
                State.MoveBackVisibility = WtfVisibility.Hidden;
                State.MoveNextVisibility = WtfVisibility.Hidden;
			}
		}

		internal void UpdateSteps()
		{
			// Update the list of steps
			Steps[CurrentStep].Instruction = State.Instruction;
			Steps[CurrentStep].PicturePath = State.PicturePath;
		}

		internal void NewProcess()
		{
			Steps = new List<Step>();
			NewStep();
			Transition(0);
		}

		internal void NewStep()
		{
			Steps.Add(new Step(""));
		}

		internal void DeleteStep()
		{
			if (editMode)
			{
				if (!Steps.Remove(Steps[CurrentStep]))
				{
					MessageBox.Show("Unable to delete step!");
				}

				if (Steps.Count == 0)
				{
					NewStep();
				}

				// Transition to the next step (or the previous step if deleting the last step)
				if (CurrentStep >= Steps.Count)
				{
					Transition(CurrentStep - 1);
				}
				else
				{
					Transition(CurrentStep);
				}
			}
		}

		internal void DeleteAllSteps()
		{
			Steps.Clear();
			NewStep();
		}

		internal void DeletePicture()
		{
			if (editMode)
			{
				State.PicturePath = "";
				Steps[CurrentStep].PicturePath = "";
			}
		}

		internal void MoveStepBack()
		{
			if (editMode)
			{
				if (CurrentStep <= 0)
				{
					return;
				}

				Steps.Insert(CurrentStep - 1, Steps[CurrentStep]);
				Steps.RemoveAt(CurrentStep + 1);
				Transition(CurrentStep - 1);
			}
		}

		internal void MoveStepNext()
		{
			if (editMode)
			{
				if (CurrentStep + 2 > Steps.Count)
				{
					return;
				}
				else if (CurrentStep + 2 == Steps.Count)
				{
					Steps.Add(Steps[CurrentStep]);
				}
				else
				{
					Steps.Insert(CurrentStep + 2, Steps[CurrentStep]);
				}

				Steps.RemoveAt(CurrentStep);
				Transition(CurrentStep + 1);
			}
		}

		internal void InsertPicture(string picturePath)
		{
			if (editMode)
			{
				string extension = System.IO.Path.GetExtension(picturePath);
				if (extension == ".jpg" || extension == ".png" || extension == ".bmp")
				{
					Steps[CurrentStep].PicturePath = picturePath;
					Transition(CurrentStep);
				}
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void Transition(int nextStep)
		{
			if (nextStep < 0 || nextStep >= Steps.Count)
			{
				return;
			}

			CurrentStep = nextStep;

			// Update the GUI with a new instruction, picture, and number
			State.Instruction = Steps[CurrentStep].Instruction;
			State.Number = "Step " + (CurrentStep + 1) + " of " + Steps.Count;
			State.PicturePath = Steps[CurrentStep].PicturePath;

			// Update the buttons for first and last steps
			State.BackEnabled = CurrentStep <= 0 ? false : true;
			State.MoveBackEnabled = State.BackEnabled;
			State.NextEnabled = CurrentStep >= Steps.Count - 1 ? false : true;
			State.MoveNextEnabled = State.NextEnabled;
			State.CancelText = CurrentStep >= Steps.Count - 1 ? "Finish" : "Cancel";

			if (editMode)
			{
				State.NextEnabled = true;
			}
		}

		private bool ExecuteStep()
		{
			// TODO - Should it return true if it didn't run?
			// Do not run the command in edit mode
			if (editMode)
			{
				return true;
			}
			else
			{
				return RunCommand(Steps[CurrentStep].Command);
			}
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
