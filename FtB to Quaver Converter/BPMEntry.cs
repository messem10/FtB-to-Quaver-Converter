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
		string bpm;
		string startTime;

		private static char[] separators = new char[] { ' ' };

		public BPMEntry(string newBPM, string newStartTime)
		{
			bpm = newBPM;
			startTime = newStartTime;
		}

		public BPMEntry(string bpmEntry)
		{
			string[] tempArray = bpmEntry.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			// tempArray[0] is always "BPM" without quotes
			startTime = tempArray[1];
			bpm = tempArray[2];
		}

		public void ExportBPMToQuaver(StreamWriter sw)
		{
			sw.WriteLine("- StartTime: " + startTime);
			sw.WriteLine("  Bpm: " + bpm);
		}
	}
}
