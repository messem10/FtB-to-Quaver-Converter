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
				DifficultyTextBox.Text = GetFileNameFromDirectory(openFileDlg.FileName).Split('.')[0];
			}
		}

		private void AudioBrowseButton_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension  
			openFileDlg.DefaultExt = ".mp3";
			openFileDlg.Filter = "MP3 file (.mp3)|*.mp3";

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				AudioFileNameTextBox.Text = openFileDlg.FileName;
			}
		}

		private void BackgroundBrowseButton_Click(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog
			Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

			// Set filter for file extension and default file extension  
			openFileDlg.DefaultExt = ".jpg";
			openFileDlg.Filter = "JPEG file (.jpg)|*.jpg";

			// Launch OpenFileDialog by calling ShowDialog method
			Nullable<bool> result = openFileDlg.ShowDialog();
			// Get the selected file name and display in a TextBox.
			// Load content of file in a TextBlock
			if (result == true)
			{
				BackgroundFileNameTextBox.Text = openFileDlg.FileName;
			}
		}
		#endregion

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (!CanConvert())
				return;
			
			Convert();
		}


		private void Convert()
		{
			StreamReader sr = new StreamReader(InputFileNameTextBox.Text);
			StreamWriter sw = new StreamWriter(InputFileNameTextBox.Text.Replace(".txt", ".qua"));

			Chart chart = new Chart(
				GetFileNameFromDirectory(AudioFileNameTextBox.Text),
				"0",
				GetFileNameFromDirectory(BackgroundFileNameTextBox.Text),
				TitleTextBox.Text,
				ArtistTextBox.Text,
				SourceTextBox.Text,
				TagsTextBox.Text,
				CreatorTextBox.Text,
				DifficultyTextBox.Text
			);

			bool success = chart.ProcessInputFile(sr);
			if(success)
			{
				chart.MakeChartValid();
				chart.ExportChart(sw);
				MessageBox.Show("Success!\nFile was written to the same folder as the input.");
			}
			else
			{
				MessageBox.Show("Incorrect FtB Chart File.\nPlease use the game file not the editor file.", "Wrong FtB file type", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool CanConvert()
		{
			if (InputFileNameTextBox.Text == null)
			{
				MessageBox.Show("No input game file was provided.", "No input file", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}

			if (AudioFileNameTextBox.Text == null)
			{
				MessageBox.Show("No audio file was provided.", "No audio file", MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}



			return true;
		}

		private string GetFileNameFromDirectory(string directory)
		{
			return directory.Split('\\').Last();
		}
	}
}
