namespace SimQLTask
{
	public struct QuerryFunc
	{
		public readonly string Name;
		public readonly string Args;

		public QuerryFunc(string name, string args)
		{
			Name = name;
			Args = args;
		}
	}
}