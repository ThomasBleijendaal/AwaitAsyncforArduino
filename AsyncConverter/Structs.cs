namespace AsyncConverter
{
	public struct Variable
	{
		public string name;
		public string type;
		public string value;
	}

	public struct Method
	{
		public string name;
		public string returnType;
		public string arguments;
		public string body;

		public int startingLine;
		public int endingLine;
	}
}
