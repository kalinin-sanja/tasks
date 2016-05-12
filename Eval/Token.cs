namespace EvalTask
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

		public override string ToString()
		{
			return $"[{Path}] = {Value}";
        }
	}
}