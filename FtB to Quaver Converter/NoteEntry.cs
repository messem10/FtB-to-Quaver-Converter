using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FtB_to_Quaver_Converter
{
	public class NoteEntry
	{
		string startTime;
		string endTime;
		string lane;

		private static char[] separators = new char[] { ' ' };

		public NoteEntry(string noteString)
		{
			string[] temp = noteString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			if(temp[0].Contains('-'))
			{
				string[] startEnd = temp[0].Split('-');
				startTime = startEnd[0];
				endTime = startEnd[1];
			}
			else
			{
				startTime = temp[0];
			}
			lane = temp[2];
		}

		public NoteEntry(string newLane, string newStartTime, string newEndTime = null)
		{
			startTime = newStartTime;
			endTime = newEndTime;
			lane = newLane;
		}

		public void ExportNoteToQuaver(StreamWriter sw)
		{
			sw.WriteLine("- StartTime: " + startTime);
			sw.WriteLine("  Lane: " + lane);
			if (endTime != null)
				sw.WriteLine("  EndTime: " + endTime); // Hold note
		}
	}
}
