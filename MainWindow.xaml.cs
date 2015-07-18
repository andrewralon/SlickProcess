using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SlickProcess;

namespace SlickProcess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Title = Application.ResourceAssembly.GetName().Name + " " + Application.ResourceAssembly.GetName().Version;

			DataContext = StateManager
				.Instance
				.State;
		}

		private void btnBack_Click(object sender, RoutedEventArgs e)
		{
			StateManager
				.Instance
				.Back();
		}

		private void btnNext_Click(object sender, RoutedEventArgs e)
		{
			StateManager
				.Instance
				.Next();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
