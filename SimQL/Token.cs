namespace SimQLTask
{
	public struct Token
	{
		public string Path;
		public string Value;

		public Token(string path, string value)
		{
			Path = path;
			Value = value;
		}
	}
}