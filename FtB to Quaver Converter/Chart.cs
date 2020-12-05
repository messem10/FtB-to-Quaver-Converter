using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FtB_to_Quaver_Converter
{
	public class Chart
	{
		public string audioFileName;
		public string songPreviewTime;
		public string backgroundFile;
		public string mapID = "-1";
		public string mapSetId;
		public string mode = "Mode: Keys7";
		public string title;
		public string artist;
		public string source;
		public string tags;
		public string creator;
		public string difficultyName;
		public string description;
		public string editorLayers = "EditorLayers: []";
		public string customAudioSamples = "CustomAudioSamples: []";
		public string soundEffects = "SoundEffects: []";

		public List<BPMEntry> bPMEntries = new List<BPMEntry>();
		public List<NoteEntry> noteEntries = new List<NoteEntry>();

		public Chart(string newAudioFileName, string newSongPreviewTime, string newBackgroundFile, 
				string newTitle, string newArtist, string newSource, string newTags, string newCreator, 
				string newDifficultyName)
		{
			audioFileName = newAudioFileName;
			songPreviewTime = newSongPreviewTime;
			backgroundFile = newBackgroundFile;
			title = newTitle;
			artist = newArtist;
			source = newSource;
			tags = newTags;
			creator = newCreator;
			difficultyName = newDifficultyName;
		}

		public Chart()
		{

		}

		public bool ProcessInputFile(StreamReader sr)
		{
			if (sr.ReadLine() == "###FILE ALREADY PARSED###")
			{
				string temp = sr.ReadLine();
				while(temp.Contains("BPM"))
				{
					bPMEntries.Add(new BPMEntry(temp));
					temp = sr.ReadLine();
				}

				while(temp != null)
				{
					noteEntries.Add(new NoteEntry(temp));
					temp = sr.ReadLine();
				}

				sr.Close();
				return true;
			}

			return false;
		}

		public void ExportChart(StreamWriter sw)
		{
			#region Header Information
			sw.WriteLine("AudioFile: " + audioFileName);
			sw.WriteLine("SongPreviewTime: 0");
			sw.WriteLine("BackgroundFile: \'\'");
			sw.WriteLine("MapID: " + mapID);
			sw.WriteLine("MapSetId: " + mapSetId);
			sw.WriteLine(mode);
			sw.WriteLine("Title: " + title);
			sw.WriteLine("Artist: " + artist);
			sw.WriteLine("Source: " + source);
			sw.WriteLine("Tags: " + tags);
			sw.WriteLine("Creator: " + creator);
			sw.WriteLine("DifficultyName: " + difficultyName);
			sw.WriteLine("Description: Created at " + (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
			sw.WriteLine(editorLayers);
			#endregion

			sw.WriteLine("TimingPoints:");
			foreach(BPMEntry entry in bPMEntries)
			{
				entry.ExportBPMToQuaver(sw);
			}

			sw.WriteLine("SliderVelocities: []");
			sw.WriteLine("HitObjects:");
			foreach(NoteEntry noteEntry in noteEntries)
			{
				noteEntry.ExportNoteToQuaver(sw);
			}

			sw.Flush();
			sw.Close();
		}
	}
}
