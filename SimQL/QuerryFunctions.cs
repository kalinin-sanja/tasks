namespace SimQLTask
{
	public static class QuerryFunctions
	{
		public static QuerryFunc Parse(string s)
		{
			var parsed = s.Split('(', ')');
			return new QuerryFunc(parsed[0].ToLower(), parsed[1]);
		}

	}
}