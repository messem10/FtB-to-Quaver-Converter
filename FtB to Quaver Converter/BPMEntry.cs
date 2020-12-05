using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FtB_to_Quaver_Converter
{
	public class BPMEntry
	{
		public int bpm;
		public int startTime;

		private static char[] separators = new char[] { ' ' };

		public BPMEntry(string newBPM, string newStartTime)
		{
			bpm = int.Parse(newBPM);
			startTime = int.Parse(newStartTime);
		}

		public BPMEntry(string bpmEntry)
		{
			string[] tempArray = bpmEntry.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			// tempArray[0] is always "BPM" without quotes
			startTime = int.Parse(tempArray[1]);
			bpm = int.Parse(tempArray[2]);
		}

		public void ExportBPMToQuaver(StreamWriter sw)
		{
			sw.WriteLine("- StartTime: " + startTime);
			sw.WriteLine("  Bpm: " + bpm);
		}
	}
}
