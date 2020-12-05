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
		public int startTime;
		public int? endTime;
		public int lane;

		private static char[] separators = new char[] { ' ' };

		public NoteEntry(string noteString)
		{
			string[] temp = noteString.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			if(temp[0].Contains('-'))
			{
				string[] startEnd = temp[0].Split('-');
				startTime = int.Parse(startEnd[0]);
				endTime = ToNullableInt(startEnd[1]);
			}
			else
			{
				startTime = int.Parse(temp[0]);
			}
			lane = int.Parse(temp[2]);
		}

		public NoteEntry(string newLane, string newStartTime, string newEndTime = null)
		{
			startTime = int.Parse(newStartTime);
			endTime = int.Parse(newEndTime);
			lane = int.Parse(newLane);
		}

		public void ExportNoteToQuaver(StreamWriter sw)
		{
			sw.WriteLine("- StartTime: " + startTime);
			sw.WriteLine("  Lane: " + lane);
			if (endTime != null)
				sw.WriteLine("  EndTime: " + endTime); // Hold note
		}

		private static int? ToNullableInt(string s)
		{
			int i;
			if (int.TryParse(s, out i)) return i;
			return null;
		}
	}
}
