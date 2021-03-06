﻿using System;
using System.Windows;
using System.Windows.Input;

namespace SlickProcess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Constructors

		public MainWindow()
		{
			InitializeComponent();

			// Handle passed in arguments
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; i++)
			{
				Console.WriteLine("Argument #" + i + ":  " + args[i]);
			}

			// If an argument is passed in, open it!
			if (args.Length > 1 && args[1] != "")
			{
				StateManager.Instance.Open(args[1]);
			}
			else
			{
				StateManager.Instance.Demo();
			}

			//chkEdit.IsChecked = StateManager.Instance.State.EditMode;

			DataContext = StateManager.Instance.State;
		}

		#endregion Constructors

		#region Public Methods

		#endregion Public Methods

		#region Private Methods

		#region Buttons

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.Back();
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.Next();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			if (StateManager.Instance.Close())
			{
				this.Close();
			}
		}

		private void btnNew_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.NewProcess();
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.DeleteStep();
		}

		private void btnMoveBack_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.MoveStepBack();
		}

		private void btnMoveNext_Click(object sender, RoutedEventArgs e)
		{
			StateManager.Instance.MoveStepNext();
		}

		private void txtDeletePicture_MouseDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
			StateManager.Instance.DeletePicture();
		}

		#endregion Buttons

		#region Handlers

		private void picUserControl_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				// Get the paths of all files dragged in
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				// Insert the first picture
				StateManager.Instance.InsertPicture(files[0]);

				// Reset the data context for the window
				DataContext = StateManager.Instance.State;
			}
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				// Get the paths of all files dragged in
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

				// Open the first file
				StateManager.Instance.Open(files[0]);

				// Reset the data context for the window
				DataContext = StateManager.Instance.State;
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left && Keyboard.Modifiers != ModifierKeys.Control)
			{
				e.Handled = true;
				StateManager.Instance.Back();
			}
			else if (e.Key == Key.Right && Keyboard.Modifiers != ModifierKeys.Control)
			{
				e.Handled = true;
				StateManager.Instance.Next();
			}
			else if (e.Key == Key.Left && Keyboard.Modifiers == ModifierKeys.Control)
			{
				e.Handled = true;
				StateManager.Instance.MoveStepBack();
			}
			else if (e.Key == Key.Right && Keyboard.Modifiers == ModifierKeys.Control)
			{
				e.Handled = true;
				StateManager.Instance.MoveStepNext();
			}
			else if (e.Key == Key.Delete)
			{
				e.Handled = true;
				StateManager.Instance.DeleteStep();
			}
			else if (e.Key == Key.F2)
			{
				e.Handled = true;
				StateManager.Instance.ToggleEditMode();
			}
		}

		#endregion Handlers

		#region Menu

		//private void mnuNew_Click(object sender, RoutedEventArgs e)
		//{
		//	StateManager.Instance.NewStep();
		//}

		//private void mnuOpen_Click(object sender, RoutedEventArgs e)
		//{
		//	StateManager.Instance.Open();
		//}

		//private void mnuExit_Click(object sender, RoutedEventArgs e)
		//{
		//	if (StateManager.Instance.Close())
		//	{
		//		this.Close();
		//	}
		//}

		//private void mnuEditMode_Click(object sender, RoutedEventArgs e)
		//{
		//	StateManager.Instance.ToggleEditMode();
		//}

		#endregion Menu

		#endregion Private Methods
	}
}
