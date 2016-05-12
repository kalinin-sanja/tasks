using System;
using System.Globalization;
using System.Linq;

namespace SimQLTask
{
	public struct QueryFunc
	{
		public readonly string Name;
		public readonly string Selector;
		public readonly int? Top;
		public readonly double? From;

		public QueryFunc(string name, string selector, int? top = null, double? @from = null)
		{
			Name = name;
			Selector = selector;
			Top = top;
			From = @from;
		}

		public static QueryFunc Parse(string s)
		{
			var separators = new[] { '(', ')', ' ' };
			var parsed = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			switch (parsed.Length)
			{

				case 6:
					return new QueryFunc(parsed[0].ToLower(), parsed[1], int.Parse(parsed[5], NumberStyles.Any, CultureInfo.InvariantCulture), double.Parse(parsed[3],NumberStyles.Any,CultureInfo.InvariantCulture));
				case 4:
					return new QueryFunc(parsed[0].ToLower(), parsed[1], int.Parse(parsed[5], NumberStyles.Any, CultureInfo.InvariantCulture));
				default:
					return new QueryFunc(parsed[0].ToLower(), parsed[1]);
			}

		}
	}
}