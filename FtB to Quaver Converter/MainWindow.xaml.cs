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
using System.IO;

namespace FtB_to_Quaver_Converter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		#region Get Open/Save Locations
		private void InputBrowseButton_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension  
			openFileDlg.DefaultExt = ".txt";
			openFileDlg.Filter = "Text documents (.txt)|*.txt";

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				InputFileNameTextBox.Text = openFileDlg.FileName;
			}
		}

		/*private void OutputBrowseButton_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog saveFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension  
			saveFileDlg.DefaultExt = ".qua";
			saveFileDlg.Filter = "Quaver chart file (.qua)|*.qua";

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = saveFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				OutputFileNameTextBox.Text = saveFileDlg.FileName;
			}
		} */
		#endregion

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (InputFileNameTextBox.Text.Length == 0)
				return;

			Convert();
		}


		private void Convert()
		{
			StreamReader sr = new StreamReader(InputFileNameTextBox.Text);
			StreamWriter sw = new StreamWriter(InputFileNameTextBox.Text.Replace(".txt", ".qua"));

			Chart chart = new Chart();
			bool success = chart.ProcessInputFile(sr);
			if(success)
			{
				chart.ExportChart(sw);
				MessageBox.Show("Success!\nFile was written to the same folder as the input.");
			}
			else
			{
				MessageBox.Show("Incorrect FtB Chart File.\nPlease use the game file not the editor file.", "Wrong FtB file type", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
