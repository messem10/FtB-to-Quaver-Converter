using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace FtB_to_Quaver_Converter
{
	public class Chart
	{
		public string audioFileName;
		public string songPreviewTime;
		public string backgroundFile;
		public string mapID = "-1";
		public string mapSetId;
		//public string mode = "Mode: Keys7";
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

		public void MakeChartValid()
		{
			List<NoteEntry> holdNotes = noteEntries.Where(note => note.endTime != null).ToList();

			List<NoteEntry> invalidHoldToHolds = noteEntries.Where(
					holdNote => noteEntries.Where(
							otherNote =>
								otherNote.lane == holdNote.lane &&
								otherNote.startTime != holdNote.startTime &&
								otherNote.endTime != null &&
								(Math.Abs((holdNote.endTime ?? int.MinValue) - otherNote.startTime) <= 36)
							).Any() == true).ToList();

			List<NoteEntry> invalidHoldToNotes = noteEntries.Where(
					holdNote => noteEntries.Where(
							otherNote =>
								otherNote.lane == holdNote.lane &&
								otherNote.startTime != holdNote.startTime &&
								otherNote.endTime == null && // Ensures Holds to Notes
								(Math.Abs((holdNote.endTime ?? int.MinValue) - otherNote.startTime) <= 72)
							).Any() == true).ToList();

			invalidHoldToHolds = AddHoldsInInvalidChords(invalidHoldToHolds);
			invalidHoldToNotes = AddHoldsInInvalidChords(invalidHoldToNotes);

			foreach (NoteEntry invalidHold in invalidHoldToHolds)
			{
				invalidHold.endTime -= MinNoteSeparation(invalidHold.endTime, 36);
			}

			foreach(NoteEntry invalidHold in invalidHoldToNotes)
			{
				invalidHold.endTime -= MinNoteSeparation(invalidHold.endTime, 54);
			}
		}

		private List<NoteEntry> AddHoldsInInvalidChords(List<NoteEntry> invalidHolds)
		{
			List<NoteEntry> otherHolds = noteEntries.Where(
					otherNote => invalidHolds.Where(
							holdNote =>
								holdNote.lane != otherNote.lane &&
								holdNote.startTime == otherNote.startTime &&
								otherNote.endTime != null &&
								holdNote.endTime == otherNote.endTime
							).Any() == true).ToList();

			return invalidHolds.Concat(otherHolds).ToList();
		}

		public int MinNoteSeparation(int? timeOfNote, int minGap)
		{
			if (timeOfNote == null || timeOfNote <= 36) return 0;

			int bpmAtTime = bPMEntries.Where(bpm => bpm.startTime <= timeOfNote).Last().bpm;
			int divisor = 1;
			int currentSeparation = int.MaxValue;

			int msPerBeat = (int)(((float)bpmAtTime /60)*1000);
			int prevValue = 0;
			while(currentSeparation > minGap)
			{
				prevValue = currentSeparation;
				currentSeparation = msPerBeat / divisor;
				divisor *= 2;
			}

			return Math.Max(prevValue, minGap);
		}

		public void ExportChart(StreamWriter sw)
		{
			#region Header Information
			sw.WriteLine("AudioFile: " + audioFileName);
			sw.WriteLine("SongPreviewTime: 0");
			sw.WriteLine("BackgroundFile: " + ((backgroundFile.Length != 0)?backgroundFile:"\'\'"));
			sw.WriteLine("MapID: " + mapID);
			sw.WriteLine("MapSetId: " + mapSetId);
			sw.WriteLine("Mode: Keys7");
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
