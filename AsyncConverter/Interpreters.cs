using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

using AsyncConverter.Generators;

namespace AsyncConverter.Interpreters
{
	public static class SourceCode
	{
		public static string[] lines;

		public static void OpenFile(string path)
		{
			if(File.Exists(path))
			{
				lines = File.ReadAllLines(path);
			}
		}

		public static void ParseFile()
		{
			ParseAsyncMethods();
		}

		public static void ParseAsyncMethods()
		{
			IEnumerable<Method> methods = GetMethods();
			List<AsyncMethod> asyncMethods = new List<AsyncMethod>();

			// comment out all normal methods
			// create AsyncMethods from Methods

			foreach (Method method in methods)
			{
				for(int i = method.startingLine; i <= method.endingLine; i++)
				{
					lines[i] = "//" + lines[i];
				}
				
				//asyncMethods.Add(AsyncMethod.CreateFromMethod(method));
			}

			/*foreach (AsyncMethod asyncMethod in asyncMethods)
			{
				string[] code = asyncMethod.ToCode().Split('\n');

				lines = lines.
			}*/
		}


		public static IEnumerable<Method> GetMethods()
		{
			Method method = new Method();
			Regex methodFinder = new Regex(@"async_([A-Za-z0-9\-_]+)[\s]+([A-Za-z0-9\-_]+)\(([A-Za-z0-9\s\-_,]*)\)[\s]*\{");
			MatchCollection methodMatches;
			Regex whileFinder = new Regex(@"while[\s]+\([\s]*true[\s]*\)[\s]*\{");

			bool methodFound = false;
			bool whileFound = false;
			int level = 0;
			int i = 0;
			int methodStartAt = 0;

			foreach(string line in lines)
			{
				if(methodFinder.IsMatch(line))
				{
					methodMatches = methodFinder.Matches(line);

					methodFound = true;
					method = new Method();
					method.name = methodMatches[0].Groups[2].Value;
					method.returnType = methodMatches[0].Groups[1].Value;
					method.arguments = methodMatches[0].Groups[3].Value;

					method.startingLine = i;
				}
				else if(methodFound && whileFinder.IsMatch(line))
				{
					whileFound = true;
					methodStartAt = i + 1;
				}

				if(line.Contains("{"))
				{
					level++;
				}

				if(line.Contains("}"))
				{
					level--;
				}

				if(methodFound && whileFound && level == 0)
				{
					int k = 0;

					string[] body = new string[i - methodStartAt - 1];

					for (int j = methodStartAt; j < i - 1; j++)
					{
						body[k++] = lines[j];
					}

					method.body = string.Join("\n", body);
					method.endingLine = i;

					yield return method;

					methodFound = false;
					whileFound = false;
				}

				i++;
			}
		}
	}
}
